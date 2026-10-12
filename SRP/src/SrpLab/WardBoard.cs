namespace SrpLab;

/// <summary>
/// Hospital ward board: tracks beds, computes acuity scores, drafts nurse handoff notes,
/// and decides which pager code to fire. Looks like "one ward concern" — it is not.
/// </summary>
public sealed class WardBoard
{
    private readonly Dictionary<int, string> _bedPatient = new();
    private readonly Dictionary<int, int> _vitalsScore = new();
    private readonly AcuityScorer _acuityScorer = new();
    private readonly PagerEscalationLog _pagerLog = new();
    private readonly HandoffNoteFormatter _handoffFormatter = new();
    private readonly WardCensusCsvWriter _censusWriter = new();

    public void AssignBed(int bed, string patientId, int heartRate, int spo2)
    {
        if (bed <= 0) throw new ArgumentOutOfRangeException(nameof(bed));
        if (string.IsNullOrWhiteSpace(patientId)) throw new ArgumentException("patient required");

        _bedPatient[bed] = patientId.Trim().ToUpperInvariant();
        _vitalsScore[bed] = _acuityScorer.Score(heartRate, spo2);
        _pagerLog.RecordIfNeeded(bed, _vitalsScore[bed]);
    }

    public int ScoreAcuity(int heartRate, int spo2)
    {
        return _acuityScorer.Score(heartRate, spo2);
    }

    public string BuildHandoffNote(int bed)
    {
        if (!_bedPatient.TryGetValue(bed, out var patient))
            return $"Bed {bed}: empty";

        return _handoffFormatter.Format(bed, patient, _vitalsScore[bed]);
    }

    public IReadOnlyList<string> DrainPagerLog()
    {
        return _pagerLog.Drain();
    }

    public string ExportCensusCsv()
    {
        return _censusWriter.Write(_bedPatient, _vitalsScore);
    }
}

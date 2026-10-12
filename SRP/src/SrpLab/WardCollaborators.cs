namespace SrpLab;

public sealed class AcuityScorer
{
    public int Score(int heartRate, int spo2)
    {
        var score = 0;
        if (heartRate > 120 || heartRate < 45) score += 4; else if (heartRate > 100) score += 2;
        if (spo2 < 90) score += 5; else if (spo2 < 94) score += 2;
        return Math.Min(score, 10);
    }
}

public sealed class HandoffNoteFormatter
{
    public string Format(int bed, string patient, int acuity) { var tone = acuity >= 8 ? "ESCALATE" : acuity >= 4 ? "WATCH" : "STABLE"; return $"[HANDOFF {DateTime.UtcNow:yyyy-MM-dd}] Bed {bed} · {patient} · acuity={acuity} · {tone}"; }
}

public sealed class PagerEscalationLog
{
    private readonly List<string> _entries = new();
    public void RecordIfNeeded(int bed, int acuity) { if (acuity >= 8) _entries.Add($"CODE-YELLOW bed={bed} at {DateTime.UtcNow:HH:mm}"); }
    public IReadOnlyList<string> Drain() { var copy = _entries.ToList(); _entries.Clear(); return copy; }
}

public sealed class WardCensusCsvWriter
{
    public string Write(IReadOnlyDictionary<int, string> patients, IReadOnlyDictionary<int, int> acuities) { var lines = new List<string> { "bed,patient,acuity" }; foreach (var bed in patients.Keys.OrderBy(x => x)) lines.Add($"{bed},{patients[bed]},{acuities[bed]}"); return string.Join('\n', lines); }
}

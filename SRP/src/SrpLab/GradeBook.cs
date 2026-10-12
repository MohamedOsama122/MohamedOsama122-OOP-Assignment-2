namespace SrpLab;

/// <summary>
/// Grade book: aggregation, letter-band policy, and transcript export.
/// </summary>
public sealed class GradeBook
{
    private readonly ScoreLedger _ledger=new(); private readonly GradeAverageCalculator _averageCalculator=new(); private readonly LetterGradePolicy _letterPolicy=new(); private readonly HonorRollPolicy _honorPolicy=new(); private readonly TranscriptFormatter _transcriptFormatter=new(); private readonly GradeBookCsvExporter _csvExporter=new();

    public void Record(string studentId, decimal score)
    {
        _ledger.Record(studentId,score);
    }

    public decimal Average(string studentId)
    {
        return _averageCalculator.Calculate(_ledger.Get(studentId));
    }

    public string Letter(string studentId)
    {
        return _letterPolicy.Get(Average(studentId));
    }

    public bool MeetsHonorRoll(string studentId)
    {
        return _honorPolicy.Meets(Average(studentId),Letter(studentId));
    }

    public string TranscriptPlain(string studentId, string fullName)
    {
        return _transcriptFormatter.Format(studentId,fullName,Average(studentId),Letter(studentId),MeetsHonorRoll(studentId));
    }

    public string ExportCsv()
    {
        return _csvExporter.Export(_ledger.StudentIds(),Average,Letter,MeetsHonorRoll);
    }
}

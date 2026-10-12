namespace SrpLab;

/// <summary>
/// Loan desk: eligibility arithmetic, required-document lists, and rejection letter prose.
/// </summary>
public sealed class LoanDesk
{
    public decimal RequestedAmount { get; }
    public int CreditScore { get; }
    public int EmploymentMonths { get; }
    public bool HasCollateral { get; }
    private readonly LoanRiskScorer _riskScorer=new(); private readonly LoanEligibilityPolicy _eligibilityPolicy=new(); private readonly RequiredDocumentPolicy _documentPolicy=new(); private readonly LoanDecisionLetterComposer _letterComposer=new(); private readonly UnderwriterCsvFormatter _csvFormatter=new();

    public LoanDesk(decimal requestedAmount, int creditScore, int employmentMonths, bool hasCollateral)
    {
        RequestedAmount = requestedAmount;
        CreditScore = creditScore;
        EmploymentMonths = employmentMonths;
        HasCollateral = hasCollateral;
    }

    public decimal RiskScore()
    {
        return _riskScorer.Score(RequestedAmount,CreditScore,EmploymentMonths,HasCollateral);
    }

    public bool IsEligible() => _eligibilityPolicy.IsEligible(RiskScore(),CreditScore);

    public IReadOnlyList<string> RequiredDocuments()
    {
        return _documentPolicy.Create(RequestedAmount,EmploymentMonths,HasCollateral,IsEligible());
    }

    public string DecisionLetter(string applicantName)
    {
        return _letterComposer.Compose(applicantName,RequestedAmount,RiskScore(),IsEligible(),RequiredDocuments());
    }

    public string UnderwriterCsvRow(string applicationId)
    {
        // Analytics export schema is yet another reason to change.
        return _csvFormatter.Format(applicationId,CreditScore,EmploymentMonths,HasCollateral,RiskScore(),IsEligible());
    }
}

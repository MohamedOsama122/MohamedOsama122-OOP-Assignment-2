namespace SrpLab;

/// <summary>
/// Subscription billing: proration math, invoice number minting, and dunning email bodies.
/// </summary>
public sealed class SubscriptionBilling
{
    private readonly SubscriptionProrationCalculator _proration=new(); private readonly InvoiceNumberGenerator _invoices=new(); private readonly DunningEmailComposer _dunning=new(); private readonly LedgerJournalFormatter _ledger=new();

    public string CustomerId { get; }
    public decimal MonthlyPrice { get; }
    public DateOnly PeriodStart { get; }
    public DateOnly PeriodEnd { get; }
    public int FailedPayments { get; private set; }

    public SubscriptionBilling(string customerId, decimal monthlyPrice, DateOnly periodStart, DateOnly periodEnd)
    {
        CustomerId = customerId;
        MonthlyPrice = monthlyPrice;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
    }

    public decimal Prorate(DateOnly activeFrom)
    {
        return _proration.Calculate(MonthlyPrice,PeriodStart,PeriodEnd,activeFrom);
    }

    public string NextInvoiceNumber()
    {
        return _invoices.Next(PeriodStart);
    }

    public void RegisterFailedPayment() => FailedPayments++;

    public string DunningEmail(string customerName, DateOnly asOf)
    {
        return _dunning.Compose(customerName,asOf,Prorate(PeriodStart),NextInvoiceNumber(),FailedPayments);
    }

    public string LedgerJournalLine(DateOnly activeFrom)
    {
        return _ledger.Format(CustomerId,NextInvoiceNumber(),Prorate(activeFrom));
    }
}

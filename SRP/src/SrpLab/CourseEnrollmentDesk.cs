namespace SrpLab;

/// <summary>
/// Course enrollment: capacity, waitlist math, welcome-packet markdown, and invoice lines.
/// </summary>
public sealed class CourseEnrollmentDesk
{
    private readonly EnrollmentRoster _roster = new(); private readonly WelcomePacketComposer _packetComposer=new(); private readonly TuitionInvoiceFormatter _invoiceFormatter=new();
    public int Capacity { get; }
    public decimal Tuition { get; }
    public string CourseCode { get; }

    public CourseEnrollmentDesk(string courseCode, int capacity, decimal tuition)
    {
        CourseCode = courseCode;
        Capacity = capacity;
        Tuition = tuition;
    }

    public string Register(string studentEmail)
    {
        if (string.IsNullOrWhiteSpace(studentEmail)) throw new ArgumentException("email");
        var email = studentEmail.Trim();

        return _roster.Register(email,Capacity);
    }

    public int WaitlistPosition(string studentEmail)
    {
        return _roster.WaitlistPosition(studentEmail);
    }

    public string WelcomePacketMarkdown(string studentEmail, string studentName)
    {
        return _packetComposer.Compose(CourseCode,_roster.IsSeated(studentEmail),WaitlistPosition(studentEmail),studentName);
    }

    public string TuitionInvoiceLine(string studentEmail)
    {
        return _invoiceFormatter.Format(CourseCode,Tuition,_roster.IsSeated(studentEmail));
    }

    public void PromoteFromWaitlist(int seats)
    {
        _roster.Promote(seats,Capacity);
    }
}

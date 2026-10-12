namespace SrpLab;

/// <summary>
/// Support ticket: SLA clocks, NLP-ish priority from free text, and public reply templates.
/// </summary>
public sealed class SupportTicket
{
    public string Id { get; }
    public string Subject { get; private set; }
    public string Body { get; private set; }
    public DateTimeOffset OpenedAt { get; }
    public string Priority { get; private set; } = "P3";
    private readonly TicketPriorityClassifier _classifier = new(); private readonly SlaDeadlinePolicy _slaPolicy = new(); private readonly PublicReplyComposer _replyComposer = new(); private readonly EscalationBlurbFormatter _escalationFormatter = new();

    public SupportTicket(string id, string subject, string body, DateTimeOffset openedAt)
    {
        Id = id;
        Subject = subject;
        Body = body;
        OpenedAt = openedAt;
        RecalculatePriorityFromText();
    }

    public void AppendCustomerMessage(string text)
    {
        Body += "\n---\n" + text;
        RecalculatePriorityFromText();
    }

    public void RecalculatePriorityFromText()
    {
        Priority = _classifier.Classify(Subject, Body);
    }

    public DateTimeOffset SlaDeadline()
    {
        return _slaPolicy.Deadline(Priority, OpenedAt);
    }

    public bool IsBreached(DateTimeOffset now) => now > SlaDeadline();

    public string DraftPublicReply(string agentName)
    {
        return _replyComposer.Compose(Id, Priority, agentName, SlaDeadline());
    }

    public string InternalEscalationBlurb()
    {
        return _escalationFormatter.Format(Id, Priority, SlaDeadline());
    }
}

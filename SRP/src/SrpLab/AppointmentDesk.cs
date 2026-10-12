namespace SrpLab;

/// <summary>
/// Appointment desk: opening-hours rules, slot search, and ICS calendar blob generation.
/// </summary>
public sealed class AppointmentDesk
{
    private readonly ClinicBusinessHoursPolicy _hoursPolicy=new(); private readonly AppointmentBookingCalendar _calendar=new(); private readonly AppointmentSlotFinder _slotFinder=new(); private readonly IcsCalendarSerializer _icsSerializer=new(); private readonly SmsReminderComposer _smsComposer=new();
    public TimeOnly Open { get; }
    public TimeOnly Close { get; }
    public int SlotMinutes { get; }

    public AppointmentDesk(TimeOnly open, TimeOnly close, int slotMinutes)
    {
        Open = open;
        Close = close;
        SlotMinutes = slotMinutes;
    }

    public bool IsWithinBusinessHours(DateTimeOffset when)
    {
        return _hoursPolicy.IsWithin(when,Open,Close,SlotMinutes);
    }

    public DateTimeOffset? FindNextSlot(DateTimeOffset from, int searchHours)
    {
        return _slotFinder.Find(from,searchHours,SlotMinutes,_hoursPolicy,Open,Close,_calendar);
    }

    public bool TryBook(DateTimeOffset slot)
    {
        if (!IsWithinBusinessHours(slot)) return false; return _calendar.Book(slot);
    }

    public string ToIcs(DateTimeOffset slot, string patientName, string clinician)
    {
        return _icsSerializer.Serialize(slot,SlotMinutes,patientName,clinician);
    }

    public string SmsReminder(DateTimeOffset slot, string clinicPhone)
    {
        return _smsComposer.Compose(slot,clinicPhone);
    }
}

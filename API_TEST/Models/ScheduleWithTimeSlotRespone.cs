using API_TEST.Data;

public class ScheduleWithTimeSlotRespone
{
    public int MediaId { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Frequency { get; set; }
    public int Interval { get; set; }
    public DateTime EndRecurrenceDate { get; set; }
    public List<DayOfWeek> DaysOfWeek { get; set; }
    public List<TimeSlot> TimeSlots { get; set; }
}
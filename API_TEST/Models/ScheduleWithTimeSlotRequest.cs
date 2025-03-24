namespace API_TEST.Models
{
    public class ScheduleWithTimeSlotRequest
    {
        public ScheduleRequest ScheduleRequest { get; set; }
        public TimeSlotRequest TimeSlotRequest { get; set; }
    }
}

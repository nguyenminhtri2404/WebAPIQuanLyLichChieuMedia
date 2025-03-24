using API_TEST.Data;

namespace API_TEST.Models
{
    public class ScheduleRespone
    {
        public int MediaId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public RecurrenceFrequency? Frequency { get; set; }
        public int? Interval { get; set; }
        public DateTime? EndRecurrenceDate { get; set; }
    }
}

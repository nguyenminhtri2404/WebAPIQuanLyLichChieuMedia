using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_TEST.Data
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleId { get; set; }

        public int MediaId { get; set; }
        public Media Media { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // Quy tắc lặp lại
        public RecurrenceFrequency? Frequency { get; set; }
        public int? Interval { get; set; }
        public DateTime? EndRecurrenceDate { get; set; }
    }


    public enum RecurrenceFrequency
    {
        Daily = 1,
        Weekly,
        Monthly,
        Yearly
    }

}

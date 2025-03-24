using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_TEST.Data
{
    public class Media
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MediaId { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string MediaType { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<TimeSlot> TimeSlots { get; set; }

    }
}

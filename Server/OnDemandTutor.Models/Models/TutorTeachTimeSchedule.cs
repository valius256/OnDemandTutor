using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnDemandTutor.Models.Models
{
    public class TutorTeachTimeSchedule : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateOnly? DayOfWeek { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public int? TutorId { get; set; }

        public int? Status { get; set; }

        [ForeignKey("TutorId")]
        [InverseProperty("TutorTeachTimeSchedules")]
        public virtual User Tutor { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Models.Models
{
    public class TutorDegree : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int? TutorId { get; set; }

        public int? DegreeImgID { get; set; }

        public string Description { get; set; }

        public virtual Medium DegreeImg { get; set; }

        public virtual User Tutor { get; set; }
    }
}

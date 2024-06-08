using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnDemandTutor.Models.Models
{
    public partial class TutorVideo : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int? TutorId { get; set; }

        public string VideoUrl { get; set; }

        public string Description { get; set; }

        [ForeignKey("TutorId")]
        [InverseProperty("TutorVideos")]
        public virtual User Tutor { get; set; }
    }
}

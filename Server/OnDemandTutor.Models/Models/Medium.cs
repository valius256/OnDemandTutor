using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnDemandTutor.Models.Models
{
    public class Medium : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreateAt { get; set; }

        [InverseProperty("DegreeImg")]
        public virtual ICollection<TutorDegree> TutorDegrees { get; set; } = new List<TutorDegree>();

        [InverseProperty("DegreeImage")]
        public virtual ICollection<User> UserDegreeImages { get; set; } = new List<User>();

        [InverseProperty("IdCardImage")]
        public virtual ICollection<User> UserIdCardImages { get; set; } = new List<User>();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnDemandTutor.Models.Models
{
    public class Subject : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string SubjectType { get; set; }

        public int? CreateBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }

        public bool Status { get; set; }

        [InverseProperty("Subject")]
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}

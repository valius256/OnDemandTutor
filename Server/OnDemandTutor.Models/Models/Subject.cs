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

        public string Description { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreateAt { get; set; }

        public bool Status { get; set; }
        public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
    }
}

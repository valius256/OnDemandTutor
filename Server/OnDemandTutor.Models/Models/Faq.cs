using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnDemandTutor.Models.Models
{
    public class FAQ : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Question { get; set; }

        public string? Answer { get; set; }

        public int CreateBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreateAt { get; set; }

        [ForeignKey("CreateBy")]
        [InverseProperty("FAQs")]
        public virtual User CreateByNavigation { get; set; }
    }
}

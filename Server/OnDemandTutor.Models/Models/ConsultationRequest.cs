using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnDemandTutor.Models.Models
{
    public class ConsultationRequest : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [StringLength(15)]
        public string Phone { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime RequestDate { get; set; }

        public int Status { get; set; }
    }
}

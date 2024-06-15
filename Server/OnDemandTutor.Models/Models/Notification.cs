using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnDemandTutor.Models.Models
{
    public class Notification : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Content { get; set; }

        public int? ReceiverId { get; set; }

        public string? RefUrl { get; set; }

        public string? RefImageUrl { get; set; }

        public int ViewStatus { get; set; }

        [ForeignKey("ReceiverId")]
        [InverseProperty("Notifications")]
        public virtual User Receiver { get; set; }
    }
}

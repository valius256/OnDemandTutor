using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Models.Dtos.FAQ
{
    public class CreateFAQDto
    {
        [Required]
        public required string Question { get; set; }
        public string? Answer { get; set; }
    }
}
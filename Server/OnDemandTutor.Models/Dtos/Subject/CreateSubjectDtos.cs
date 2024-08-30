using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Models.Dtos.Subject
{
    public class CreateSubjectDtos
    {
        [Required]
        public required string Name { get; set; }
        public string SubjectType { get; set; } = string.Empty;
        public int? CreateById { get; set; }
        public string Description { get; set; } = string.Empty ;
        public DateTime? CreateAt { get; set; }
        public bool IsEnable { get; set; }
    }
}


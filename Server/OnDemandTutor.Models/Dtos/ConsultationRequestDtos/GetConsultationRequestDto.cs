using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.ConsultationRequestDtos
{
    public class GetConsultationRequestDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string ConsultationContent { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public ConsultationRequestStatus Status { get; set; }
    }
}


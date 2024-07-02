using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.ConsultationRequestDtos;

public class HandleConsultationRequestDto
{
    public int Id { get; set; }
    public ConsultationRequestStatus Status { get; set; }
    public string? ReasonFailed { get; set; }
}
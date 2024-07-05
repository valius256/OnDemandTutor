using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.ConsultationRequestDtos;

public class ConsultationRequestFilterDto
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? ConsultationContent { get; set; }
    public DateTime? RequestDateFrom { get; set; }
    public DateTime? RequestDateTo { get; set; }
    public ConsultationRequestStatus? ConsultationStatus { get; set; }
    public int Limit { get; set; }
    public int Page { get; set; }
}
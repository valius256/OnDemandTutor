namespace OnDemandTutor.Models.Dtos.ConsultationRequestDtos;

public class ConsultationRequestFilterDto
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? ConsultationContent { get; set; }
    public DateOnly? RequestDateFrom { get; set; }
    public DateOnly? RequestDateTo { get; set; }
}
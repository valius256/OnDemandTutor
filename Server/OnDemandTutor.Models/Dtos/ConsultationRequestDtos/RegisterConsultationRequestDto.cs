namespace OnDemandTutor.Models.Dtos.ConsultationRequestDtos;

public class RegisterConsultationRequestDto
{
    public string? Name { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string ConsultationContent { get; set; } = string.Empty;

}
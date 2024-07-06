using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class ConsultationRequest : BaseEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Phone { get; set; }
    public string? Email { get; set; }
    public int? HandleById { get; set; }
    public string? ConsultationContent { get; set; }
    public DateTime RequestDate { get; set; }
    public ConsultationRequestStatus Status { get; set; }
    public virtual User? HandleBy { get; set; }

    public ConsultationRequest()
    {
        RequestDate = DateTime.UtcNow;
    }
}
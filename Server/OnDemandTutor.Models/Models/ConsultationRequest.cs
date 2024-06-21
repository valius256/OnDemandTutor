namespace OnDemandTutor.Models.Models;

public class ConsultationRequest : IBaseEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Phone { get; set; }
    public DateTime RequestDate { get; set; }
    public int Status { get; set; }
}
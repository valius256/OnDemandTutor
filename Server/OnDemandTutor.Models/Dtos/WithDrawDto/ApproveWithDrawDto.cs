using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.WithDrawDto;

public class ApproveWithDrawDto
{
    public int Id { get; set; }
    public WithDrawStatus Status { get; set; }
    public string? Reply { get; set; }
}
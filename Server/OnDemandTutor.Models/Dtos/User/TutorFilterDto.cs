using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class TutorFilterDto
{
    public string? Name { get; set; } 
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; } 
    public Sex? Sex { get; set; }
    public DateTime? DobFromDate { get; set; }
    public DateTime? DobToDate { get; set; }
    public DateTime? JoinFromDate { get; set; }
    public DateTime? JoinToDate { get; set; }
    public List<int>? Subject { get; set; }
    public int Limit { get; set; }
    public int Page { get; set; }
}
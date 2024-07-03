using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class TutorFilterDto
{
    public string? name { get; set; } = string.Empty;
    public string? email { get; set;  } = string.Empty;
    public string? phone { get; set; } = string.Empty;
    public string? Address { get; set; }= string.Empty;
    public Sex sex { get; set; }
    public DateTime? DobFromDate { get; set; }
    public DateTime? DobToDate { get; set; }
    public DateTime? JoinFromDate { get; set; }
    public DateTime? JoinToDate { get; set; } 
    public string Subject { get; set; } = string.Empty; 
    public int Limit { get; set; }
    public int Page { get; set; } 
}
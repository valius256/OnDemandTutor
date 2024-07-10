using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User;

public class GrantRoleDto
{
    public string? email { get; set; }
    public int? id  { get; set; }
    public RoleStatus Role  { get; set; }
}
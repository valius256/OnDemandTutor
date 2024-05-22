using Microsoft.AspNetCore.Identity;
using OnDemandTutor.DataAccess.Models.Enums;

namespace OnDemandTutor.DataAccess.Models
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public Guid? EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public bool IsRequestResetTwoFactor { get; set; }
        public DateTime? LastTimeUpdatePassword { get; set; }
        public UserStatus? Status { get; set; }
    }
}

using OnDemandTutor.Models.Dtos.Register;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User
{
    public class RegisterTutorDtos : RegisterDtos
    {
        public RoleStatus Role { get; set; } = RoleStatus.Tutor;
        public string? DegreeIdentityId { get; set; }
        public string? AvatarImageId { get; set; }
        public string? IdentityCardId { get; set; }
        public string? ScheduleDescription { get; set; }
    }
}

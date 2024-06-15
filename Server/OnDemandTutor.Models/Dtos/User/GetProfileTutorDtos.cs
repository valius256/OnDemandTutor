using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User
{
    public class GetProfileTutorDtos : GetProfileUserDtos
    {
        public string? DegreeIdentityId { get; set; }
        public string? AvatarImageId { get; set; }
        public string? IdentityCardId { get; set; }
        public string? ScheduleDescription { get; set; }
        public UserStatus? UserStatus { get; set; }
    }
}

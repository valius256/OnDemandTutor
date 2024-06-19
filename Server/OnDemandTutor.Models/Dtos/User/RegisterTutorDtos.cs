using OnDemandTutor.Models.Dtos.Register;

namespace OnDemandTutor.Models.Dtos.User
{
    public class RegisterTutorDtos : RegisterDtos
    {
        public string? DegreeIdentityId { get; set; }
        public string? AvatarImageId { get; set; }
        public string? IdentityCardId { get; set; }
        public string? ScheduleDescription { get; set; }
        public string diplomaNumber { get; set; }
        public DateOnly issuanceDate { get; set; }
    }
}

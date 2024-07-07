using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User
{
    public class ChangStatusDto
    {
        public int Id { get; set; }
        public TutorStatus Status { get; set; }
    }
}

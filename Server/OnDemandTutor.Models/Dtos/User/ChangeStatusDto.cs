using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.User
{
    public class ChangeStatusDto
    {
        public int Id { get; set; }
        public TutorStatus Status { get; set; }

        public string? Reason { get; set; }
    }
}

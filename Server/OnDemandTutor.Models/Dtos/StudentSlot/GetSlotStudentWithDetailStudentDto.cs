

using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.StudentSlot
{
    public class GetSlotStudentWithDetailStudentDto
    {
        public int Id { get; set; }
        public int SlotId { get; set; }

        public GetProfileUserDto User { get; set; } = new GetProfileUserDto();
        public PaymentStatus PaymentStatus { get; set; }
        public decimal? Rating { get; set; }
        public string? Feedback { get; set; }

        public decimal PaidValue { get; set; } = 0;
        public bool IsTransferred { get; set; } = false;

    }
}

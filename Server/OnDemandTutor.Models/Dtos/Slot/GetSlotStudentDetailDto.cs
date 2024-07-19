

using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Slot
{
    public class GetSlotStudentDetailDto
    {
        public int Id { get; set; }
        public GetSlotDetailDto Slot { get; set; } = new GetSlotDetailDto();

        public GetSimpleUserDto User { get; set; } = new GetSimpleUserDto();
        public PaymentStatus PaymentStatus { get; set; }
        public decimal? Rating { get; set; }
        public string? Feedback { get; set; }
    }
}

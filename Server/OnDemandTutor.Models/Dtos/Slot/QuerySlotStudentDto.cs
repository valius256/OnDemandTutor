
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.BusinessLogic.Services.Slot
{
    public class QuerySlotStudentDto
    {
        public DateTime From { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        public DateTime To { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1,1);
        public PaymentStatus? PaymentStatus { get; set; }
    }
}

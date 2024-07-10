

using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.WithDrawDto
{
    public class GetRequestWithdrawDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string BankAccountNumber { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? OperatorId { get; set; }
        public string? Reply { get; set; }
        public WithDrawStatus Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public GetProfileUserDtos User { get; set; } = new GetProfileUserDtos();
        public GetSimpleUserDto? Operator { get; set; } = new GetSimpleUserDto();
    }
}

using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos
{
    public class GetProfileUserDtos
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public required string Email { get; set; }
        public string? Address { get; set; }

        public int? AvatarImageId { get; set; }

        public string? Password { get; set; }

        public RoleStatus Role { get; set; }

        public DateTime? Dob { get; set; }

        public string? Gender { get; set; }

        public string? BankAccount { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Models.Dtos.User
{
    public class UpdateUserDtos
    {
        [EmailAddress(ErrorMessage = "The email format is not valid")]
        public string Mail { get; set; }
        public string? Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "The passwords didn't match.")]
        public required string ConfirmPassword { get; set; }
        [Phone(ErrorMessage = "The phone format is not valid")]
        public string Phone { get; set; }
    }
}

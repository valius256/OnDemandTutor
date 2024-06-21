using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Models.Dtos.Authen;

public class LoginDtos
{
    [EmailAddress(ErrorMessage = "The email format is not valid")]
    public required string Email { get; set; }

    public required string Password { get; set; }
}
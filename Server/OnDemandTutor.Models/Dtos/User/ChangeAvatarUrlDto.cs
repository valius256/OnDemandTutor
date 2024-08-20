using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Models.Dtos.User;

public class ChangeAvatarUrlDto
{
    [Required]
    public required string Url { get; set; }
}
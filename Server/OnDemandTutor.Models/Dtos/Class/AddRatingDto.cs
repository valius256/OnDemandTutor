using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Models.Dtos.Class;

public class AddRatingDto
{
    public int ClassId { get; set; }

    [Range(1, 5)] public int Rating { get; set; }

    public string? Feedback { get; set; }
}
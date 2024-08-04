using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Models.Dtos.Slot;

public class CreateSlotsDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string TeachAddress { get; set; } = string.Empty;
    public int? SubjectId { get; set; }
    public bool IsOnline { get; set; }

    [Range(1, 100, ErrorMessage = "Number of student must be from 1 to 100")]
    public int NumberOfStudents { get; set; }
}
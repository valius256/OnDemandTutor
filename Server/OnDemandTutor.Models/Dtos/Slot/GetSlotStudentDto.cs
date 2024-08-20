using System.ComponentModel.DataAnnotations;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.SlotStudent;

public class GetSlotStudentDto
{
    public int SlotId { get; set; }
    public int UserId { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string Feedback { get; set; } = string.Empty;
    [Range(1, 5)]
    public decimal? Rating { get; set; }
}
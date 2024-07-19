using System.ComponentModel.DataAnnotations;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.SlotStudent;

public class SlotStudentDto
{
    public int SlotId { get; set; }
    public int UserId { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string Feedback { get; set; }
    [Range(1, 5)]
    public decimal? Rating { get; set; }
}
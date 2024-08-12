using OnDemandTutor.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Models.Models;

public class SlotStudent : BaseEntity
{
    public int SlotId { get; set; }
    public virtual Slot Slot { get; set; } = default!;

    public int UserId { get; set; }
    public virtual User User { get; set; } = default!;

    public PaymentStatus PaymentStatus { get; set; }
    public string? Feedback { get; set; }
    [Range(1, 5)]
    public decimal? Rating { get; set; }

    public decimal PaidValue { get; set; } = 0;
    public bool IsTransferred { get; set; } = false;
}


namespace OnDemandTutor.Models.Enum;

public enum SlotStatus
{
    NotYet,  
    OnGoing, // trừ Customer lúc bắt đầu khóa 
    Cancelled,
    Finished, // chuyển cho Tutor sau 1 ngày
}
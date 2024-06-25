namespace OnDemandTutor.Models.Models;

public class FAQ : BaseEntity
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string? Answer { get; set; }
    public int CreateById { get; set; }
    public DateTime CreateAt { get; set; }
    public virtual User CreateBy { get; set; }
}
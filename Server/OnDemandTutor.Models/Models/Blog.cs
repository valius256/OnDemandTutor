namespace OnDemandTutor.Models.Models;

public class Blog : BaseEntity
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public int CreateById { get; set; }

    public DateTime CreateAt { get; set; }

    public int? UpdateById { get; set; }

    public DateTime? UpdateAt { get; set; }
    public string? Thumbnail { get; set; }

    public virtual User? CreateBy { get; set; }

    public virtual User? UpdateBy { get; set; }
}
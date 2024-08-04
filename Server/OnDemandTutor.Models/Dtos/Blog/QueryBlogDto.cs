namespace OnDemandTutor.Models.Dtos.Blog;

public class QueryBlogDto
{
    public string? Keyword { get; set; }
    public DateTime? CreateFrom { get; set; }
    public DateTime? CreateTo { get; set; }
    public DateTime? UpdateFrom { get; set; }
    public DateTime? UpdateTo { get; set; }
    public int? CreateBy { get; set; }
    public bool? IsHidden { get; set; }
}
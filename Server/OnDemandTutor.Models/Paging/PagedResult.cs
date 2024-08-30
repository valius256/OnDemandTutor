namespace OnDemandTutor.Models.Paging;

public class PagedResult<T>
{
    public IList<T> Items { get; set; } = new List<T>();
    public string[] Errors { get; set; } = [];
    public int Limit { get; set; }
    public int Page { get; set; }
    public int Total { get; set; }
}
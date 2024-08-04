namespace OnDemandTutor.Models.Dtos.Blog;

public class GetBlogDtos
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public int CreateById { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool? IsHidden { get; set; }

    public int? UpdateById { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Thumbnail { get; set; }

    public virtual UserDto? CreateBy { get; set; }

    public virtual UserDto? UpdateBy { get; set; }

    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Name => FirstName + " " + LastName;
    }
}
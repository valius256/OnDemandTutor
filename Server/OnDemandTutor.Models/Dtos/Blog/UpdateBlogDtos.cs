namespace OnDemandTutor.Models.Dtos.Blog
{
    public class UpdateBlogDtos
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public int CreateById { get; set; }

        public DateTime CreateAt { get; set; }

        public bool? IsHidden { get; set; }

        public int? UpdateById { get; set; }

        public DateTime? UpdateAt { get; set; }

        public string? Thumbnail { get; set; }

        public virtual string? CreateBy { get; set; }

        public virtual string? UpdateBy { get; set; }
    }
}


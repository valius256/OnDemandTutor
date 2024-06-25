namespace OnDemandTutor.Models.Dtos.Blog
{
    public class UpdateBlogDtos
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UpdateById { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}


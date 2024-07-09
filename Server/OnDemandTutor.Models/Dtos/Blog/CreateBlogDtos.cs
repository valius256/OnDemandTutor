namespace OnDemandTutor.Models.Dtos.Blog
{
    public class CreateBlogDtos
    {
        
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public bool? IsHidden { get; set; }

        public string? Thumbnail { get; set; }
    }
}


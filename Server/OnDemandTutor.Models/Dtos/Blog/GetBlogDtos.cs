namespace OnDemandTutor.Models.Dtos.Blog
{
    public class GetBlogDtos
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public int CreateById { get; set; }

        public DateTime CreateAt { get; set; }

        public int? UpdateById { get; set; }

        public DateTime? UpdateAt { get; set; }

        public virtual UserDto? CreateBy { get; set; }

        public virtual UserDto? UpdateBy { get; set; }

        public class UserDto
        {
            public int Id { get; set; }
            public string Username { get; set; }
        }
    }
}


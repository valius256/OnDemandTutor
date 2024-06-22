using System;
namespace OnDemandTutor.Models.Dtos.Blog
{
	public class CreateBlogDtos
	{
        public string Title { get; set; }
        public string Content { get; set; }
        public int CreateById { get; set; }
        public DateTime CreateAt { get; set; }
    }
}


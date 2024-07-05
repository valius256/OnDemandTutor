using System;
namespace OnDemandTutor.Models.Dtos.FAQ
{
    public class FAQDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string? Answer { get; set; }
        public int CreateById { get; set; }
        public DateTime CreateAt { get; set; }
        public string? CreateByName { get; set; } // Assuming you want to include the name of the creator
    }
}


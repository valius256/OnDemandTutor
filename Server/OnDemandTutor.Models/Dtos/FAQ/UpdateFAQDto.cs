namespace OnDemandTutor.Models.Dtos.FAQ
{
    public class UpdateFAQDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string? Answer { get; set; }
    }
}
namespace OnDemandTutor.Models.Dtos.ConsultationRequestDtos
{
    public class GetConsultationRequestDtos
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Phone { get; set; }
        public string RequestContent { get; set; }
        public DateOnly RequestDate { get; set; }
        public int Status { get; set; }
    }
}


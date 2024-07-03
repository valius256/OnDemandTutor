namespace OnDemandTutor.Models.Dtos.ConsultationRequestDtos
{
    public class GetConsultationRequestDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Phone { get; set; }
        public string ConsultationContent { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}


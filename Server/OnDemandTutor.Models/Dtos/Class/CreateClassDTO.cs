namespace OnDemandTutor.Models.Dtos.Class
{
	public class CreateClassDTO
	{
        public string? Name { get; set; }
        public int TutorId { get; set; }
        public int SubjectId { get; set; }
        public string? StudentName { get; set; }
        public string? Location { get; set; }
        public string? Method { get; set; }
        public List<int> SlotIds { get; set; }
    }
}


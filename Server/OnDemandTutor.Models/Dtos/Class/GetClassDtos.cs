using System;
namespace OnDemandTutor.Models.Dtos.Class
{
	public class GetClassDtos
	{
        public int Id { get; set; }
        public string? Name { get; set; }
        public int TutorId { get; set; }
        public int SubjectId { get; set; }
        public string? StudentName { get; set; }
        public int SlotId { get; set; }
    }
}


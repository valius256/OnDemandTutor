using System;
namespace OnDemandTutor.Models.Dtos.Subject
{
	public class UpdateClassDtos
	{
        public int Id { get; set; }
        public string Name { get; set; }

        public string SubjectType { get; set; }

        public int? CreateBy { get; set; }

        public string Description { get; set; }

        public DateTime? CreateAt { get; set; }

        public bool Status { get; set; }

    }
}


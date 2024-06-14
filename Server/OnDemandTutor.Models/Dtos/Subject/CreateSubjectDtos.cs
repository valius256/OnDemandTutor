using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnDemandTutor.Models.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnDemandTutor.Models.Dtos.Subject
{
	public class CreateSubjectDtos
	{
        public string Name { get; set; }

        public string SubjectType { get; set; }

        public int? CreateBy { get; set; }

        public string Description { get; set; }

        public DateTime? CreateAt { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
    }
	
}


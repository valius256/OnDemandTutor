using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.TutorSubject
{
    public class CreateTutorSubjectDto
    {
        public int UserId { get; set; }
        public int SubjectId { get; set; }
        public SubjectStatus Status { get; set; }
    }
}
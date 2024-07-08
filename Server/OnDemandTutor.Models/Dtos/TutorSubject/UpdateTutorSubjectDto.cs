using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.TutorSubject
{
    public class UpdateTutorSubjectDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SubjectId { get; set; }
        public TutorSubjectStatus Status { get; set; }
    }
}
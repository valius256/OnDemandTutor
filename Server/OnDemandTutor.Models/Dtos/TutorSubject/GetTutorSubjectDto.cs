using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.TutorSubject
{
    public class GetTutorSubjectDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public SubjectStatus Status { get; set; }
    }
}
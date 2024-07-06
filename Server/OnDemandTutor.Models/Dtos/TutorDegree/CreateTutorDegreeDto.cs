using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.TutorDegree
{
    public class CreateTutorDegreeDto
    {
        public int? TutorId { get; set; }
        public string? DegreeImgUrl { get; set; }
        public string Description { get; set; }
        public int SubjectId { get; set; }
        public string DegreeNumber { get; set; }
        public DateOnly IssuranceDate { get; set; }
        public TutorSubjectDegreeStatus TutorSubjectStatus { get; set; }
        public string? RejectReason { get; set; }

    }
}
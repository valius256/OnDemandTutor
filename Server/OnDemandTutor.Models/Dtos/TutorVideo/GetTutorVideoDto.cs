using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTutor.Models.Dtos.TutorVideo
{
    public class GetTutorVideoDto
    {
        public int Id { get; set; }
        public int? TutorId { get; set; }
        public string TutorName { get; set; }
        public string VideoUrl { get; set; }
        public string Description { get; set; }
    }
}
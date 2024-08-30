using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Class
{
    public class QueryClassDTO
    {
        public string? Name { get; set; }
        public int? SubjectId { get; set; }
        public string? UserName { get; set; }
        public int? TutorId { get; set; }
        public string? Address { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? MinFeePerHour { get; set; }
        public decimal? MaxFeePerHour { get; set; }
        public string? Method { get; set; }

        public List<ClassStatus> Status { get; set; } = new List<ClassStatus>();
    }
}


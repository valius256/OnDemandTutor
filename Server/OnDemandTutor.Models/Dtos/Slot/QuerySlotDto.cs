namespace OnDemandTutor.Models.Dtos.Slot
{
    public class QuerySlotDto
    {
        public int? ClassId { get; set; }
        public int? UserId { get; set; }

        public int? SubjectId { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

    }
}

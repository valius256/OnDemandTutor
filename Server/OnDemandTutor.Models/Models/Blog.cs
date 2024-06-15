namespace OnDemandTutor.Models.Models
{
    public class Blog : IBaseEntity
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public int CreateBy { get; set; }

        public DateTime CreateAt { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateAt { get; set; }

        public virtual User CreateByUser { get; set; }

        public virtual User UpdateByUser { get; set; }
    }
}

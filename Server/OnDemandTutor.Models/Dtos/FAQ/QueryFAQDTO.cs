using System;
namespace OnDemandTutor.Models.Dtos.FAQ
{
	public class QueryFAQDTO
	{
        public string? Keyword { get;set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public DateTime? CreateFrom { get; set; }
        public DateTime? CreateTo { get; set; }
        public DateTime? UpdateFrom { get; set; }
        public DateTime? UpdateTo { get; set; }
        public int? CreateBy { get; set; }
    }
}


using OnDemandTutor.Models.Dtos.User;
namespace OnDemandTutor.Models.Dtos.FAQ
{
    public class GetFAQDto
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string? Answer { get; set; }
        //public int CreateById { get; set; } 
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public GetSimpleUserDto CreateBy { get; set; } = new GetSimpleUserDto();
        //public string? CreateByName { get; set; } // Assuming you want to include the name of the creator
    }
}


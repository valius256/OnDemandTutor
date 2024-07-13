using OnDemandTutor.Models.Dtos.User;
using System;
namespace OnDemandTutor.Models.Dtos.FAQ
{
    public class FAQDTO

    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string? Answer { get; set; }
        //public int CreateById { get; set; } 
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public GetSimpleUserDto CreateBy { get; set;} = new GetSimpleUserDto();
        //public string? CreateByName { get; set; } // Assuming you want to include the name of the creator
    }
}


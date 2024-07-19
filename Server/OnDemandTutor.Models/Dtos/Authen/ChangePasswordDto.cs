using System;
namespace OnDemandTutor.Models.Dtos.Authen
{
	public class ChangePasswordDto
	{
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}


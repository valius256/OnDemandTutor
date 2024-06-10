using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.Dtos.Authen;

namespace OnDemandTutor.BusinessLogic.Interfaces.Auth
{
    public interface IAuthServices
    {
        Task<AuthResponseDto> Login(LoginDtos loginDto);
        Task<string> LoginWithFireBase(LoginDtos loginDto);
    }
}

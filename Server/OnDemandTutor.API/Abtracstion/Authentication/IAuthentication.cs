using OnDemandTutor.Models.Dtos.Authen;

namespace OnDemandTutor.API.Abtracstion.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(LoginDtos loginDtos);

    }
}

using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.Dtos.Authen;

namespace OnDemandTutor.BusinessLogic.Interfaces.Auth;

public interface IJwtProviderServices
{
    Task<AuthenResponseDto> GetForCredentialsAsync(string email, string password);
}
using OnDemandTutor.Models.Dtos.Register;

namespace OnDemandTutor.BusinessLogic.Interfaces.Auth
{
    public interface IFireBaseAuthServices
    {
        Task<string> RegisterUser(RegisterDtos registerDtos);
        Task<string> ForgotPassword(string email);
    }
}

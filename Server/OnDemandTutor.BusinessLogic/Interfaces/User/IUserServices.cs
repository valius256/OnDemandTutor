using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.Dtos.Register;

namespace OnDemandTutor.BusinessLogic.Interfaces.User
{
    public interface IUserServices
    {
        Task<GetProfileUserDtos> RegisterUser(RegisterDtos registerDtos);
        Task<GetProfileUserDtos> VerifyLogin(string? email, string? password);
        Task<List<GetProfileUserDtos>> GetAllUsers();
    }
}

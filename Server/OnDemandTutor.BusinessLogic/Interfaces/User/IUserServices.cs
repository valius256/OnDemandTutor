using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.Dtos.Register;
using OnDemandTutor.Models.Dtos.User;

namespace OnDemandTutor.BusinessLogic.Interfaces.User
{
    public interface IUserServices
    {
        Task<GetProfileUserDtos> RegisterUser(RegisterDtos registerDtos);
        Task<GetProfileUserDtos> VerifyLogin(string? email, string? password);
        Task<List<GetProfileUserDtos>> GetAllUsers();
        Task<GetProfileUserDtos> GetProfile(int? userId, string? email);
        Task<GetProfileTutorDtos> RegisterTutor(RegisterTutorDtos registerTutorDtos);
        Task<GetProfileUserDtos> GetUserProfileById(int id);
        Task<bool> DeleteUserAsync(string? email);
        //Task<GetProfileUserDtos> UpdateProfile(UpdateProfileUserDtos updateProfileUserDtos);
    }
}

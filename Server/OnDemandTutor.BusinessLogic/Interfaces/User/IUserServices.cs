using System.Security.Claims;
using FirebaseAdmin.Auth;
using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.Dtos.Register;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.User;

public interface IUserServices
{
    Task<GetProfileUserDtos> RegisterUser(RegisterDtos registerDtos);
    Task<GetProfileUserDtos> VerifyLogin(string? email, string? password);
    Task<List<GetProfileUserDtos>> GetAllUsers();
    Task<GetProfileUserDtos> GetProfile(int? userId, string? email);
    Task<GetProfileTutorDtos> RegisterTutor(RegisterTutorDtos registerTutorDtos, ClaimsPrincipal UserPrincipal);
    Task<GetProfileUserDtos> GetUserProfileById(int id);
    Task<GetProfileUserDtos> GetUserProfileByFireBaseId(string uId);

    Task<bool> DeleteUserAsync(string? email);

    //Task<GetProfileUserDtos> UpdateProfile(UpdateProfileUserDtos updateProfileUserDtos);
    Task<bool> SyncUserAsync(List<ExportedUserRecord> listUserFireData);
    Task<List<TutorRegistrationRequestDtos>> LoadTutorRegistrationList();
    Task<PagedResult<TutorSimpleProfileDtos>> ViewTutorList(PagingModel<TutorSimpleProfileRequest> request);
    Task<List<TutorRegistrationResponseDtos>> ApprovedTutorRegistration(TutorRegistrationRequestDtos requestDtos, ClaimsPrincipal claims);

}
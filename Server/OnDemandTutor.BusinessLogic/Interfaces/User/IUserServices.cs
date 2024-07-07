using FirebaseAdmin.Auth;
using OnDemandTutor.Models.Dtos.Register;
using OnDemandTutor.Models.Dtos.User;
using System.Security.Claims;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.User;

public interface IUserServices
{
    Task<GetProfileUserDtos> RegisterUser(RegisterDtos registerDtos);
    Task<GetProfileUserDtos> VerifyLogin(string? email, string? password);
    Task<PagedResult<GetProfileUserDtos>> GetAllUsers(UserFilterDto request);
    Task<GetProfileUserDtos> GetProfile(int? userId, string? email);
    Task<GetProfileTutorDtos> RegisterTutor(RegisterTutorDtos registerTutorDtos, ClaimsPrincipal UserPrincipal);
    Task<GetProfileUserDtos> GetUserProfileById(int id);
    Task<GetProfileUserDtos> GetUserProfileByFireBaseId(string uId);
    Task<bool> RechargeAccount(int uId, decimal money);
    Task<bool> DeleteUserAsync(string? email);

    //Task<GetProfileUserDtos> UpdateProfile(UpdateProfileUserDtos updateProfileUserDtos);
    Task<bool> SyncUserAsync(List<ExportedUserRecord> listUserFireData);
    Task<List<TutorRegistrationRequestDtos>> LoadTutorRegistrationList();
    Task<PagedResult<TutorSimpleProfileDto>> ViewTutorList(TutorFilterDto request);
    Task<bool> ApprovedTutorRegistration(TutorRegistrationRequestDtos requestDtos, ClaimsPrincipal claims);
    Task<bool> DeleteTutor(DeleteTutorDto requestDto);
    Task<bool> UpdateProfile(UpdateUserDto requestDto, ClaimsPrincipal claims);
    Task<bool> UpdateAvatarImage(string imageUrl, ClaimsPrincipal claims);
    Task<decimal?> GetBalanceAsync(int userId);
    Task<bool> UpdateBalance(int userId, decimal amount);
    Task<bool> DeaActiveAccount(DeaActiveAccountDto request);
    Task<bool> ActiveAccount(int id);
    Task<CompareStatusDto> ChangeTutorStatus(int id, TutorStatus status);

}
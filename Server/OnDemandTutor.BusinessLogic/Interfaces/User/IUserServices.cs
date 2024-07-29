using FirebaseAdmin.Auth;
using OnDemandTutor.Models.Dtos.Register;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Paging;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Interfaces.User;

public interface IUserServices
{
    Task<GetProfileUserDtos> RegisterUser(RegisterDtos registerDtos);
    Task<GetProfileUserDtos> VerifyLogin(string? email, string? password);
    Task<PagedResult<GetProfileUserDtos>> GetAllUsersAsync(UserFilterDto request, GetProfileUserDtos? accessor);
    Task<GetProfileUserDtos> GetProfileAsync(int? userId, string? email, GetProfileUserDtos? accessor);
    //Task<GetProfileTutorDtos> RegisterTutorAsync(RegisterTutorDtos registerTutorDtos, GetProfileUserDtos user);
    Task<GetProfileUserDtos> GetUserByIdAsync(int? userId);
    Task<GetProfileUserDtos> GetUserByEmailAsync(string email);
    Task<GetProfileUserDtos> GetUserProfileByIdAsync(int id);
    Task<GetProfileUserDtos> GetUserProfileByFireBaseIdAsync(string uId);

    Task<GetUserBalanceDto> GetUserBalanceAsync(int? userId);
    Task<bool> RechargeAccountAsync(int uId, decimal money);
    Task<bool> DeleteUserAsync(string? email);

    //Task<GetProfileUserDtos> UpdateProfile(UpdateProfileUserDtos updateProfileUserDtos);
    Task<bool> SyncUserAsync(List<ExportedUserRecord> listUserFireData);
    Task<List<TutorRegistrationRequestDtos>> LoadTutorRegistrationList();
    Task<PagedResult<TutorSimpleProfileDto>> ViewTutorListAsync(TutorFilterDto request);
    //Task<bool> ApprovedTutorRegistrationAsync(TutorRegistrationRequestDtos requestDtos, ClaimsPrincipal claims);
    Task<bool> DeleteTutorAsync(DeleteTutorDto requestDto);
    Task<bool> UpdateProfileAsync(UpdateUserDto requestDto, GetProfileUserDtos user);
    Task<bool> UpdateAvatarImage(string imageUrl, GetProfileUserDtos user);
    Task<decimal?> GetBalanceAsync(int userId);
    Task<bool> UpdateBalanceAsync(int userId, decimal moneyIncrease, decimal moneyDecrease);
    Task<bool> DeaActiveAccountAsync(DeaActiveAccountDto request);
    Task<bool> ActiveAccount(int id);
    Task<CompareStatusDto> ChangeTutorStatus(int id, TutorStatus status);
    Task<bool> RecalculateTutorRating(int tutorId);
    Task<PagedResult<GetOutstandingTutorDto>> GetOutstandingTutor(int limit, int page);

    Task<List<GetSimpleUserDto>> GetAllOperators();
    Task<bool> UpdateTutorRating(GetProfileUserDtos tutorProfile);

}
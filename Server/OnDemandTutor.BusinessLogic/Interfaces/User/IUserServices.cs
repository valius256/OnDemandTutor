using FirebaseAdmin.Auth;
using OnDemandTutor.Models.Dtos.Register;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Paging;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Interfaces.User;

public interface IUserServices
{
    Task<GetProfileUserDto> RegisterUser(RegisterDtos registerDtos);
    Task<GetProfileUserDto> VerifyLogin(string? email, string? password);
    Task<PagedResult<GetProfileUserDto>> GetAllUsersAsync(UserFilterDto request, GetProfileUserDto? accessor);
    Task<GetProfileUserDto> GetProfileAsync(int? userId, string? email, GetProfileUserDto? accessor);
    Task<GetProfileUserDto> GetUserByIdAsync(int? userId);
    Task<GetProfileUserDto> GetUserByEmailAsync(string email);
    Task<GetProfileUserDto> GetUserProfileByIdAsync(int id);
    Task<GetProfileUserDto> GetUserProfileByFireBaseIdAsync(string uId);

    Task<GetUserBalanceDto> GetUserBalanceAsync(int? userId);
    Task<bool> RechargeAccountAsync(int uId, decimal money);
    Task<bool> DeleteUserAsync(string? email);

    Task<bool> SyncUserAsync(List<ExportedUserRecord> listUserFireData);
    Task<PagedResult<TutorSimpleProfileDto>> ViewTutorListAsync(TutorFilterDto request);
    Task<bool> DeleteTutorAsync(DeleteTutorDto requestDto);
    Task<bool> UpdateProfileAsync(UpdateUserDto requestDto, GetProfileUserDto user);
    Task<bool> UpdateAvatarImage(string imageUrl, GetProfileUserDto user);
    Task<decimal?> GetBalanceAsync(int userId);
    Task<bool> UpdateBalanceAsync(int userId, decimal money);
    Task<bool> DeaActiveAccountAsync(DeaActiveAccountDto request);
    Task<bool> ActiveAccount(int id);
    Task<CompareStatusDto> ChangeTutorStatus(ChangeStatusDto request);
    Task<bool> RecalculateTutorRating(int tutorId);
    Task<PagedResult<GetOutstandingTutorDto>> GetOutstandingTutor(int limit, int page);

    Task<List<GetSimpleUserDto>> GetAllOperators();
    Task<bool> UpdateTutorRating(GetProfileUserDto tutorProfile);

}

using FirebaseAdmin.Auth;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Mail;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Register;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Services.User;

public class UserServices : IUserServices
{
    private readonly IFireBaseAuthServices _fireBaseAuthServices;
    private readonly IEmailServices _emailServices;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;

    public UserServices(IUnitOfWorkRepository unitOfWorkRepository, IFireBaseAuthServices fireBaseAuthServices, IEmailServices emailServices, INotificationService notificationService)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
        _fireBaseAuthServices = fireBaseAuthServices;
        _emailServices = emailServices;
        _notificationService = notificationService;
    }

    public async Task<PagedResult<GetProfileUserDto>> GetAllUsersAsync(UserFilterDto request, GetProfileUserDto? accessor)
    {
        var userList = await _unitOfWorkRepository.UserRepository.ViewUsersListAsync(request);
        if (accessor == null || (accessor.Role != RoleStatus.Operator && accessor.Role != RoleStatus.Admin))
        {
            userList.Items.ToList().ForEach(u => u.Balance = 0);
        }
        return userList.Adapt<PagedResult<GetProfileUserDto>>();
    }

    public async Task<GetProfileUserDto> GetProfileAsync(int? userId, string? userEmail, GetProfileUserDto? accessor)
    {
        var userModel = await GetUserByIdAsync(userId);
        if (accessor == null || (accessor.Role != RoleStatus.Operator && accessor.Role != RoleStatus.Admin))
        {
            userModel.Balance = 0;
        }
        return userModel.Adapt<GetProfileUserDto>();
    }
    public async Task<GetProfileUserDto> GetUserByIdAsync(int? userId)
    {
        var userModel =
            await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Id == userId);
        if (userModel == null) throw new BadRequestException("User not found");
        return userModel.Adapt<GetProfileUserDto>();
    }
    public async Task<GetProfileUserDto> GetUserByEmailAsync(string email)
    {
        var userModel =
            await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Email == email);
        if (userModel == null) throw new BadRequestException("User not found");
        return userModel.Adapt<GetProfileUserDto>();
    }

    public async Task<GetUserBalanceDto> GetUserBalanceAsync(int? userId)
    {
        var userModel =
            await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Id == userId);
        if (userModel == null) throw new BadRequestException("User not found");

        return userModel.Adapt<GetUserBalanceDto>();
    }

    public async Task<GetProfileUserDto> RegisterUser(RegisterDtos registerDtos)
    {
        var userExist =
            await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(us => us.Email == registerDtos.Email);
        if (userExist != null)
        {
            throw new ModelException("Email", $"{userExist.Email} already exists, try logging in",
              "This Email is already registered");
        }
        else
        {
            var fireBaseAuthId = await _fireBaseAuthServices.RegisterUser(registerDtos);
            // Hash the password
            // using var hmac = new HMACSHA512();
            // var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDtos.Password)));
            var mappedUser = registerDtos.Adapt<Models.Models.User>();
            mappedUser.Role = RoleStatus.Customer;
            mappedUser.FireBaseid = fireBaseAuthId;
            mappedUser.CreatedDate = DateTime.Now;
            mappedUser.Balance = 0;
            if (registerDtos.isTutor)
            {
                mappedUser.Role = RoleStatus.Tutor;
               

                
            }

            // mappedUser.Password = passwordHash; // open when present 
            var addedUser = await _unitOfWorkRepository.UserRepository.AddAsync(mappedUser);

            await _unitOfWorkRepository.SaveChangesAsync();

            var rs = addedUser.Entity.Adapt<GetProfileUserDto>();
            if (registerDtos.isTutor)
            {
                await _notificationService.CreateNotificationAsync(new Models.Dtos.Notification.CreateNotificationDto
                {
                    Content = "Bạn đã đăng kí thành công! Nhưng để tài khoản có thể hoạt động thì bạn cần hoàn chỉnh hồ sơ. Click vào đây để thực hiện",
                    ReceiverIds = new List<int> { rs.Id },
                    RefUrl = "/tutor/profile",
                    RefImageUrl = "/src/assets/logo.png"
                });
                await _emailServices.SendEmailAsync(new List<string> { mappedUser.Email }, new List<string>(),
                   "Chào mừng đến với OnDemandTutor",
                   "<h1>Chúc mừng bạn đã đăng ký thành công</h1>Gửi " + mappedUser.FirstName + " " + mappedUser.LastName + " yêu dấu!.<br>Chỉ còn bước cuối cùng để tài khoản bạn có thể hoạt động được trên nền tảng, đó là hoàn chỉnh hồ sơ và đăng ảnh giấy tờ tùy thân nên. Hãy vô mục profile cá nhân của mình để thực hiện nhé!<br>Một lần nữa cảm ơn bạn vì đã tham gia cùng chúng tôi!",
                   true, true);
            }
            return rs;
        }

    }


    public async Task<GetProfileUserDto> VerifyLogin(string? email, string? password)
    {
        if (email.IsNullOrEmpty())
            if (email != null)
                throw new ModelException(email, "Input Email or Phone number is empty");
        if (password.IsNullOrEmpty()) throw new BadRequestException("Input password is empty");
        var user = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u =>
            u.Email == email && u.Password.Equals(password));

        if (user is null) throw new DataNotFoundException("Wrong Email, Phone number or password");

        return user.Adapt<GetProfileUserDto>();
    }


    public async Task<GetProfileUserDto> GetUserProfileByIdAsync(int id)
    {
        return (await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Id == id))
            .Adapt<GetProfileUserDto>();
    }

    public async Task<GetProfileUserDto> GetUserProfileByFireBaseIdAsync(string uId)
    {
        return (await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.FireBaseid == uId))
            .Adapt<GetProfileUserDto>();
    }

    public async Task<bool> RechargeAccountAsync(int uId, decimal money)
    {
        await UpdateBalanceAsync(uId, money);
        return true;
    }

    public async Task<bool> DeleteUserAsync(string? email)
    {
        var user = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(ld => ld.Email == email);
        if (user == null) throw new DataNotFoundException("User not found");
        _unitOfWorkRepository.UserRepository.Remove(user);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SyncUserAsync(List<ExportedUserRecord> listUserFireData)
    {
        var usersToSync = listUserFireData.Adapt<List<Models.Models.User>>();

        // Fetch existing users' Firebase IDs
        var existingUsers = await _unitOfWorkRepository.UserRepository.ToListAsync();
        var existingUserIds = new HashSet<string>(existingUsers.Select(u => u.FireBaseid));

        // Filter out users that already exist
        var newUsers = usersToSync.Where(u => !existingUserIds.Contains(u.FireBaseid)).ToList();
        newUsers.ForEach(newUser => newUser.Password = string.Empty);
        // Add new users
        if (newUsers.Any()) await _unitOfWorkRepository.UserRepository.AddRangeAsync(newUsers);

        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }


    public async Task<PagedResult<TutorSimpleProfileDto>> ViewTutorListAsync(TutorFilterDto request)
    {
        return (await _unitOfWorkRepository.UserRepository.ViewTutorListAsync(request)).Adapt<PagedResult<TutorSimpleProfileDto>>();
    }


    public async Task<bool> DeleteTutorAsync(DeleteTutorDto requestDto)
    {
        var userInDb = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(ld => ld.Id == requestDto.userId);
        if (userInDb == null) throw new DataNotFoundException("User not found");
        if (userInDb.IsActive == false)
        {
            throw new ModelException("Tutor status", $"{userInDb.IsActive} already delete",
                "This account is already deleted");
        }

        await _unitOfWorkRepository.UserRepository.Where(ld => ld.Id == requestDto.userId)
            .ExecuteUpdateAsync(setter => setter.SetProperty(s => s.IsActive, false)
                                                    .SetProperty(s => s.RecordStatus, RecordStatus.Inactive)
                                                    .SetProperty(s => s.TutorStatus, TutorStatus.Banned)
                                                    .SetProperty(s => s.DeaActiveReason, requestDto.DeaActiveReason)

            );
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateProfileAsync(UpdateUserDto requestDto, GetProfileUserDto user)
    {

        if (requestDto.Id == null || requestDto.Id == 0)
        {
            requestDto.Id = user.Id;
        }

        var userInDb = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(l => l.Id == requestDto.Id);
        if (userInDb == null)
        {
            throw new DataNotFoundException("User not found.");
        }
        if (userInDb.Id != requestDto.Id && userInDb.Role < RoleStatus.Operator)
        {
            throw new BadRequestException("You do not have permission to do this!");
        }

        UpdateUserFields(userInDb, requestDto);

        _unitOfWorkRepository.UserRepository.Update(userInDb);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    private void UpdateUserFields(Models.Models.User userInDb, UpdateUserDto requestDto)
    {
        if (!string.IsNullOrEmpty(requestDto.FirstName))
        {
            userInDb.FirstName = requestDto.FirstName;
        }
        if (!string.IsNullOrEmpty(requestDto.LastName))
        {
            userInDb.LastName = requestDto.LastName;
        }
        if (!string.IsNullOrEmpty(requestDto.Phone))
        {
            userInDb.Phone = requestDto.Phone;
        }
        if (!string.IsNullOrEmpty(requestDto.Email))
        {
            userInDb.Email = requestDto.Email;
        }
        if (!string.IsNullOrEmpty(requestDto.Address))
        {
            userInDb.Address = requestDto.Address;
        }
        if (!string.IsNullOrEmpty(requestDto.AvatarImageUrl))
        {
            userInDb.AvatarImageUrl = requestDto.AvatarImageUrl;
        }
        if (requestDto.Dob.HasValue)
        {
            userInDb.Dob = requestDto.Dob.Value;
        }
        if (requestDto.Sex.HasValue)
        {
            userInDb.Sex = requestDto.Sex.Value;
        }
        if (requestDto.TutorFeePerHour.HasValue)
        {
            userInDb.TutorFeePerHour = requestDto.TutorFeePerHour.Value;
        }
        if (!string.IsNullOrEmpty(requestDto.IdCardImageUrl))
        {
            userInDb.IdCardImageUrl = requestDto.IdCardImageUrl;
        }
        if (!string.IsNullOrEmpty(requestDto.ScheduleDesciption))
        {
            userInDb.ScheduleDesciption = requestDto.ScheduleDesciption;
        }

    }

    public async Task<bool> UpdateAvatarImage(string imageUrl, GetProfileUserDto user)
    {
        var userInDb = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(l => l.Id == user.Id);
        if (userInDb == null) throw new DataNotFoundException("User not found");
        userInDb.AvatarImageUrl = imageUrl;
        _unitOfWorkRepository.UserRepository.Update(userInDb);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    public async Task<decimal?> GetBalanceAsync(int userId)
    {
        var record = await _unitOfWorkRepository.UserRepository
            .FirstOrDefaultAsync(ld => ld.Id == userId);
        return record?.Balance ?? 0;
    }

    public async Task<bool> UpdateBalanceAsync(int userId, decimal money)
    {
        var record = await _unitOfWorkRepository.UserRepository
            .FirstOrDefaultAsync(ld => ld.Id == userId);
        if (record == null) throw new DataNotFoundException("User not found");
        record.Balance += money;
        
        if (record.Balance < 0)
        {
            throw new ModelException($"{record.Balance}", "The balance cannot be negative",
                "The balance cannot be negative");
        }

        _unitOfWorkRepository.UserRepository.Update(record);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeaActiveAccountAsync(DeaActiveAccountDto request)
    {
        var modelInDb = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(ld => ld.Id == request.Id);
        if (modelInDb == null) throw new ArgumentNullException(nameof(modelInDb));

        if (modelInDb.IsActive == false)
            throw new ModelException($"{modelInDb.IsActive}", "is not active", "not_active");

        modelInDb.IsActive = false;
        modelInDb.DeaActiveReason = request.DeaActiveReason;
        _unitOfWorkRepository.UserRepository.Update(modelInDb);
        await _unitOfWorkRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ActiveAccount(int id)
    {
        await _unitOfWorkRepository.UserRepository.Where(ld => ld.Id == id)
                                                        .ExecuteUpdateAsync(setter => setter
                                                            .SetProperty(s => s.IsActive, true)
                                                            .SetProperty(s => s.TutorStatus, TutorStatus.Un_Verified)
                                                            .SetProperty(s => s.RecordStatus, RecordStatus.Active)
                                                            .SetProperty(s => s.DeaActiveReason, String.Empty)

                                                        );
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    public async Task<CompareStatusDto> ChangeTutorStatus(ChangeStatusDto request)
    {
        var oldRecord = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Id == request.Id);
        if (oldRecord == null)
        {
            throw new ArgumentException("Tutor not found");
        }

        // Validate state transitions
        bool isValidTransition = false;
        string noti_message = "";
        switch (oldRecord.TutorStatus)
        {
            case TutorStatus.Un_Verified:
                if (request.Status == TutorStatus.Sent_Verification_Requested)
                {
                    noti_message = "Yêu cầu xác mình của bạn đã được gửi đi. Chúng tôi sẽ phản hồi trong vòng 48h!";
                    isValidTransition = true;
                }
                break;

            case TutorStatus.Sent_Verification_Requested:
                if (request.Status == TutorStatus.Verified || request.Status == TutorStatus.Verification_Request_Rejected)
                {
                    if (request.Status == TutorStatus.Verified)
                    {
                        noti_message = "Tài khoản của bạn đã được xác minh thành công! Giờ đây bạn có thể hoạt động trên nền tảng và truy cập được các tính năng của gia sư! Chúc bạn mọi sự tốt đẹp";
                    } else
                    {
                        noti_message = "Tài khoản của bạn đã bị từ chối yêu xác minh. Lý do : '" + request.Reason + "'. Bạn có thể hoàn chỉnh và gửi lại yêu cầu";
                    }
                    isValidTransition = true;
                }
                break;

            case TutorStatus.Verification_Request_Rejected:
                if (request.Status == TutorStatus.Sent_Verification_Requested)
                {
                    noti_message = "Yêu cầu xác mình của bạn đã được gửi đi. Chúng tôi sẽ phản hồi trong vòng 48h!";
                    isValidTransition = true;
                }
                break;

            case TutorStatus.Verified:
                if (request.Status == TutorStatus.Banned || request.Status == TutorStatus.Un_Verified)
                {
                    if (request.Status == TutorStatus.Un_Verified)
                    {
                        noti_message = "Bạn cần xác minh lại tài khoản để có thể tiếp tục hoạt động. Lý do : '" +request.Reason + '"';
                    } 
                    isValidTransition = true;
                }
                break;

            case TutorStatus.Banned:
                break;
        }

        if (!isValidTransition)
        {
            throw new BadRequestException($"Invalid status transition from {oldRecord.TutorStatus} to {request.Status}");
        }

        await _unitOfWorkRepository.UserRepository.Where(u => u.Id == request.Id)
            .ExecuteUpdateAsync(setter => setter.SetProperty(s => s.TutorStatus, request.Status));

        var newRecord = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Id == request.Id);
        if (newRecord == null) throw new DataNotFoundException("The record was deleted during execution");
        await _notificationService.CreateNotificationAsync(new Models.Dtos.Notification.CreateNotificationDto
        {
            Content = noti_message,
            ReceiverIds = new List<int> { newRecord.Id },
            RefUrl = "/tutor/profile",
            RefImageUrl = "/src/assets/logo.png"
        });

        return new CompareStatusDto()
        {
            OldStatus = oldRecord.TutorStatus,
            NewStatus = newRecord.TutorStatus
        };
    }

    public async Task<bool> RecalculateTutorRating(int tutorId)
    {
        var result = await _unitOfWorkRepository.UserRepository.RecalculateTutorRating(tutorId);
        if (result)
        {
            await _unitOfWorkRepository.SaveChangesAsync();
        }
        return result;
    }


    public async Task<PagedResult<GetOutstandingTutorDto>> GetOutstandingTutor(int limit, int page)
    {
        return await _unitOfWorkRepository.UserRepository.GetOutStandingTutors(limit, page);
    }

    public async Task<List<GetSimpleUserDto>> GetAllOperators()
    {
        var operators = await _unitOfWorkRepository.UserRepository.WhereAsync(u => u.Role == RoleStatus.Operator || u.Role == RoleStatus.Admin);
        return operators.Adapt<List<GetSimpleUserDto>>();
    }

    public async Task<bool> UpdateTutorRating(GetProfileUserDto tutorProfile)
    {
        _unitOfWorkRepository.UserRepository.Update(tutorProfile.Adapt<Models.Models.User>());
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }
}
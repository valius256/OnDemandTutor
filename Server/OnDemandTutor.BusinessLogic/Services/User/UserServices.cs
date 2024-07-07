using FirebaseAdmin.Auth;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Mail;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Register;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;
using System.Security.Claims;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.User;

public class UserServices : IUserServices
{
    private readonly IFireBaseAuthServices _fireBaseAuthServices;
    private readonly IMailServices _mailServices;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;

    public UserServices(IUnitOfWorkRepository unitOfWorkRepository, IFireBaseAuthServices fireBaseAuthServices, IMailServices mailServices)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
        _fireBaseAuthServices = fireBaseAuthServices;
        _mailServices = mailServices;
    }

    public async Task<PagedResult<GetProfileUserDtos>> GetAllUsers(UserFilterDto request)
    {
        var userList = await _unitOfWorkRepository.UserRepository.ViewUsersListAsync(request);
        return userList.Adapt<PagedResult<GetProfileUserDtos>>();
    }

    public async Task<GetProfileUserDtos> GetProfile(int? userId, string? userEmail)
    {
        var userModel =
            await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Id == userId || u.Email == userEmail);
        if (userModel == null) throw new BadRequestException("User not found");

        return userModel.Adapt<GetProfileUserDtos>();
    }


    public async Task<GetProfileUserDtos> RegisterUser(RegisterDtos registerDtos)
    {
        // var userInFirebase = await _fireBaseAuthServices.GetUserAsync(null, registerDtos.Email, null);
        // if (userInFirebase != null)
        // {
        //     throw new ModelException("Email", $"{userInFirebase.Email} has already registered", "This Email is already registered");
        // }

        var userExist =
            await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(us => us.Email == registerDtos.Email);
        if (userExist != null)
            throw new ModelException("Email", $"{userExist.Email} already exists, try logging in",
                "This Email is already registered");

        var fireBaseAuthId = await _fireBaseAuthServices.RegisterUser(registerDtos);

        // Hash the password
        // using var hmac = new HMACSHA512();
        // var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDtos.Password)));
        var mappedUser = registerDtos.Adapt<Models.Models.User>();
        mappedUser.Role = RoleStatus.Customer;
        mappedUser.FireBaseid = fireBaseAuthId;
        mappedUser.CreatedDate = DateTime.Now;
        // mappedUser.Password = passwordHash; // open when present 
        await _unitOfWorkRepository.UserRepository.AddAsync(mappedUser);

        await _unitOfWorkRepository.SaveChangesAsync();

        var rs = mappedUser.Adapt<GetProfileUserDtos>();
        return rs;
    }


    public async Task<GetProfileUserDtos> VerifyLogin(string? email, string? password)
    {
        if (email.IsNullOrEmpty())
            if (email != null)
                throw new ModelException(email, "Input Email or Phone number is empty");
        if (password.IsNullOrEmpty()) throw new BadRequestException("Input password is empty");
        var user = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u =>
            u.Email == email && u.Password.Equals(password));

        if (user is null) throw new NotFoundException("Wrong Email, Phone number or password");

        return user.Adapt<GetProfileUserDtos>();
    }


    public async Task<GetProfileTutorDtos> RegisterTutor(RegisterTutorDtos registerTutorDtos, ClaimsPrincipal userPrincipal)
    {
        var userUid = userPrincipal.FindFirst(c => c.Type == "user_id")?.Value;
        var userInDb = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(l => l.FireBaseid == userUid);

        if (userInDb == null)
        {
            throw new Exception("User not found.");

        }
        userInDb.AvatarImageUrl = registerTutorDtos.AvatarImageurl;
        userInDb.ScheduleDesciption = registerTutorDtos.ScheduleDescription;
        userInDb.IdCardImageUrl = registerTutorDtos.IdentityCardUrl;
        userInDb.Role = RoleStatus.Tutor;
        userInDb.TutorStatus = TutorStatus.Un_Verified;
        
        if (userInDb.AvatarImageUrl == registerTutorDtos.AvatarImageurl)
        {
            throw new ModelException("AvatarImageUrl", "AvatarImageUrl is dupplicated", "AvatarImageUrl is dupplicated");
        }

        if (userInDb.IdCardImageUrl == registerTutorDtos.IdentityCardUrl)
            throw new ModelException("IdCardImageUrl", "IdCardImageUrl is dupplicated", "IdCardImageUrl is dupplicated");

        _unitOfWorkRepository.UserRepository.Update(userInDb);
        // Assign degrees to the tutor
        foreach (var degreeDto in registerTutorDtos.Degrees)
        {
            if (userInDb.TutorDegrees.Any(ld => ld.DegreeImgUrl == degreeDto.DegreeImgUrl))
            {
                throw new Exception("Degree image is duplicated, add another link");
            }
            var tutorDegree = new TutorDegree
            {
                TutorId = userInDb.Id,
                DegreeNumber = degreeDto.DegreeNumber,
                SubjectId = degreeDto.SubjectId,
                IssuranceDate = degreeDto.IssuranceDate,
                DegreeImgUrl = degreeDto.DegreeImgUrl,
                TutorSubjectStatus = TutorSubjectDegreeStatus.Pending
            };
            userInDb.TutorDegrees.Add(tutorDegree);
        }

        await _unitOfWorkRepository.SaveChangesAsync();

        var result = userInDb.Adapt<GetProfileTutorDtos>();
        return result;
    }

    public async Task<GetProfileUserDtos> GetUserProfileById(int id)
    {
        return (await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Id == id))
            .Adapt<GetProfileUserDtos>();
    }

    public async Task<GetProfileUserDtos> GetUserProfileByFireBaseId(string uId)
    {
        return (await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.FireBaseid == uId))
            .Adapt<GetProfileUserDtos>();
    }

    public async Task<bool> RechargeAccount(int uId, decimal money)
    {
        var recordInDb = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Id == uId);

        recordInDb.Balance += money;
        _unitOfWorkRepository.UserRepository.Update(recordInDb);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(string? email)
    {
        var user = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(ld => ld.Email == email);
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

        // Add new users
        if (newUsers.Any()) await _unitOfWorkRepository.UserRepository.AddRangeAsync(newUsers);

        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }


    public async Task<List<TutorRegistrationRequestDtos>> LoadTutorRegistrationList()
    {
        var listTutorWithDegree = await _unitOfWorkRepository.UserRepository.GetUsersListDegreeData();
        return listTutorWithDegree.Adapt<List<TutorRegistrationRequestDtos>>();
    }

    public async Task<PagedResult<TutorSimpleProfileDto>> ViewTutorList(TutorFilterDto request)
    {
        return await _unitOfWorkRepository.UserRepository.ViewTutorListAsync(request);
    }

    public async Task<bool> ApprovedTutorRegistration(TutorRegistrationRequestDtos requestDtos, ClaimsPrincipal userPrincipal)
    {
        if (!requestDtos.tutorRegistrationDtos.Any())
        {
            return false;
        }
        foreach (var dto in requestDtos.tutorRegistrationDtos)
        {
            await _unitOfWorkRepository.TutorDegreeRepository
                .Where(td => td.Id == dto.TutorDegreeId)
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(s => s.TutorSubjectStatus, requestDtos.StatusApproved)
                    .SetProperty(s => s.RejectReason, dto.RejectReason)
                );

            var tutorId = await _unitOfWorkRepository.TutorDegreeRepository
                .Where(td => td.Id == dto.TutorDegreeId)
                .Select(ld => ld.TutorId)
                .FirstOrDefaultAsync();


            var tutorEmailDb = await _unitOfWorkRepository.UserRepository
                .Where(ld => ld.Id == tutorId)
                .Select(ld => ld.Email)
                .FirstOrDefaultAsync();
            var tutorEmails = new List<string>();

            if (tutorEmailDb != null)
            {
                tutorEmails.Add(tutorEmailDb);
            }

            var emailParams = new Dictionary<string, string>()
            {
                // { "TutorName", $"{user.Email}" }, for testing( using the Email can receive mail)
                { "TutorName", $"{tutorEmailDb}" },
                { "ApprovalStatus", $"{requestDtos.StatusApproved}" },
                { "RejectionReason", $"{requestDtos.tutorRegistrationDtos.FirstOrDefault()?.RejectReason}" },
            };

            await _mailServices.SendAsync(EmailType.Tutor_Registration_Approval, tutorEmails, new List<string>(), emailParams);
        }
        return true;
    }

    public async Task<bool> DeleteTutor(DeleteTutorDto requestDto)
    {
        var userInDb = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(ld => ld.Id == requestDto.userId);
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

    public async Task<bool> UpdateProfile(UpdateUserDto requestDto, ClaimsPrincipal userClaims)
    {
        var userid = userClaims.FindFirst(c => c.Type == "id")?.Value;
        if (requestDto.Id is 0 or null)
        {
            requestDto.Id = int.Parse(userid);
        }
        var userInDb = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(l => l.Id == requestDto.Id);
        _unitOfWorkRepository.UserRepository.Update(userInDb);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAvatarImage(string imageUrl, ClaimsPrincipal claims)
    {
        var userid = claims.FindFirst(c => c.Type == "id")?.Value;
        var userInDb = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(l => userid != null && l.Id == int.Parse(userid));
        userInDb.AvatarImageUrl = imageUrl;
        return true;
    }

    public async Task<decimal?> GetBalanceAsync(int userId)
    {
        var record = await _unitOfWorkRepository.UserRepository
            .FirstOrDefaultAsync(ld => ld.Id == userId);
        return record.Balance;
    }

    public async Task<bool> UpdateBalance(int userId, decimal amount)
    {
        var record = await _unitOfWorkRepository.UserRepository
            .FirstOrDefaultAsync(ld => ld.Id == userId);
        record.Balance = amount;
        _unitOfWorkRepository.UserRepository.Update(record);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeaActiveAccount(DeaActiveAccountDto request)
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

    public async Task<CompareStatusDto> ChangeTutorStatus(int id, TutorStatus newStatus)
    {
        var oldRecord = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Id == id);
        if (oldRecord == null)
        {
            throw new ArgumentException("Tutor not found");
        }

        // Validate state transitions
        bool isValidTransition = false;

        switch (oldRecord.TutorStatus)
        {
            case TutorStatus.Un_Verified:
                if (newStatus == TutorStatus.Sent_Verification_Requested)
                    isValidTransition = true;
                break;
        
            case TutorStatus.Sent_Verification_Requested:
                if (newStatus == TutorStatus.Verified || newStatus == TutorStatus.Verification_Request_Rejected)
                    isValidTransition = true;
                break;
        
            case TutorStatus.Verification_Request_Rejected:
                if (newStatus == TutorStatus.Un_Verified)
                    isValidTransition = true;
                break;
        
            case TutorStatus.Verified:
                if (newStatus == TutorStatus.Banned)
                    isValidTransition = true;
                break;

            case TutorStatus.Banned:
                break;
        }

        if (!isValidTransition)
        {
            throw new InvalidOperationException($"Invalid status transition from {oldRecord.TutorStatus} to {newStatus}");
        }
        
        await _unitOfWorkRepository.UserRepository.Where(u => u.Id == id)
            .ExecuteUpdateAsync(setter => setter.SetProperty(s => s.TutorStatus, newStatus));

        var newRecord = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Id == id);

        return new CompareStatusDto()
        {
            OldStatus = oldRecord.TutorStatus,
            NewStatus = newRecord.TutorStatus
        };
    }
}
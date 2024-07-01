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
using OnDemandTutor.Models.Paging;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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

    public async Task<List<GetProfileUserDtos>> GetAllUsers()
    {
        var userList = await _unitOfWorkRepository.UserRepository.ToListAsync();
        return userList.Adapt<List<GetProfileUserDtos>>();
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
                throw new ModelException(email, "Input Email or phone number is empty");
        if (password.IsNullOrEmpty()) throw new BadRequestException("Input password is empty");
        var user = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u =>
            u.Email == email && u.Password.Equals(password));

        if (user is null) throw new NotFoundException("Wrong email, phone number or password");

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

    public async Task<PagedResult<TutorSimpleProfileDtos>> ViewTutorList(PagingModel<TutorSimpleProfileRequest> request)
    {
        var tutorList = await _unitOfWorkRepository.UserRepository.ViewTutorListAsync(request);
        return tutorList.Adapt<PagedResult<TutorSimpleProfileDtos>>();
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
                // { "TutorName", $"{user.Email}" }, for testing( using the email can receive mail)
                { "TutorName", $"{tutorEmailDb}" },
                { "ApprovalStatus", $"{requestDtos.StatusApproved}" },
                { "RejectionReason", $"{requestDtos.tutorRegistrationDtos.FirstOrDefault()?.RejectReason}" },
            };
        
            await _mailServices.SendAsync(EmailType.Tutor_Registration_Approval, tutorEmails, new List<string>(), emailParams);
        }
        return true;
    }

    public async Task<bool> DeleteTutor(DeleteTutorDtos requestDtos)
    {
        var userInDb = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(ld => ld.Id == requestDtos.userId);
        if (userInDb.IsActive == false)
        {
            throw new ModelException("user status", $"{userInDb.IsActive} already delete",
                "This account is already deleted");
        }

        await _unitOfWorkRepository.UserRepository.Where(ld => ld.Id == requestDtos.userId)
            .ExecuteUpdateAsync(setter => setter.SetProperty(s => s.IsActive, false)
                                                    .SetProperty(s => s.RecordStatus, RecordStatus.Inactive)
                                                    .SetProperty(s => s.DeaActiveReason, requestDtos.DeaActiveReason)
                                                
            );
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateProfile(UpdateUserDtos requestDtos, ClaimsPrincipal userClaims)
    {
        var userid = userClaims.FindFirst(c => c.Type == "id")?.Value;
        if (requestDtos.Id is 0 or null)
        {
            requestDtos.Id = int.Parse(userid);
        }
        var userInDb = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(l => l.Id == requestDtos.Id);
       var rs =  requestDtos.Adapt(userInDb);
        _unitOfWorkRepository.UserRepository.Update(userInDb);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }
    
}
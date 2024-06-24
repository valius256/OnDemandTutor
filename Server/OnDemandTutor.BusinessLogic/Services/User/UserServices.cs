using FirebaseAdmin.Auth;
using Mapster;
using Microsoft.IdentityModel.Tokens;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
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
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;

    public UserServices(IUnitOfWorkRepository unitOfWorkRepository, IFireBaseAuthServices fireBaseAuthServices)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
        _fireBaseAuthServices = fireBaseAuthServices;
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
        if (email.IsNullOrEmpty()) throw new ModelException(email, "Input Email or phone number is empty");
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
        return await _unitOfWorkRepository.UserRepository.GetTutorListAsync(request);
    }

    public async Task<List<TutorRegistrationResponseDtos>>  ApprovedTutorRegistration(TutorRegistrationRequestDtos requestDtos, ClaimsPrincipal userPrincipal)
    {
        var userUid = userPrincipal.FindFirst(c => c.Type == "user_id")?.Value;

        var userWithPendingRegistration = await _unitOfWorkRepository.UserRepository.GetTutorRegistration(userUid);

        foreach (var degreeSubjectRegistration in userWithPendingRegistration)
        {
            degreeSubjectRegistration.Status = requestDtos.StatusApproved;

            _unitOfWorkRepository.SaveChangesAsync();
        }

        return userWithPendingRegistration;
    }
}
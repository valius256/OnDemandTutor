using FirebaseAdmin.Auth;
using Mapster;
using Microsoft.IdentityModel.Tokens;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.Dtos.Register;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.BusinessLogic.Services.User
{
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IFireBaseAuthServices _fireBaseAuthServices;

        public UserServices(IUserRepository userRepository, IUnitOfWorkRepository unitOfWorkRepository, IFireBaseAuthServices fireBaseAuthServices)
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
            var userModel = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Id == userId || u.Email == userEmail);
            if (userModel == null)
            {
                throw new BadRequestException("User not found");
            }

            return userModel.Adapt<GetProfileUserDtos>();
        }


        public async Task<GetProfileUserDtos> RegisterUser(RegisterDtos registerDtos)
        {
            // var userInFirebase = await _fireBaseAuthServices.GetUserAsync(null, registerDtos.Email, null);
            // if (userInFirebase != null)
            // {
            //     throw new ModelException("Email", $"{userInFirebase.Email} has already registered", "This Email is already registered");
            // }

            var userExist = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(us => us.Email == registerDtos.Email);
            if (userExist != null)
            {
                throw new ModelException("Email", $"{userExist.Email} already exists, try logging in", "This Email is already registered");
            }

            var fireBaseAuthId = await _fireBaseAuthServices.RegisterUser(registerDtos);

            var mappedUser = registerDtos.Adapt<Models.Models.User>();
            mappedUser.Role = RoleStatus.Customer;
            mappedUser.FireBaseid = fireBaseAuthId;
            await _unitOfWorkRepository.UserRepository.AddAsync(mappedUser);

            await _unitOfWorkRepository.SaveChangesAsync();

            var rs = mappedUser.Adapt<GetProfileUserDtos>();
            return rs;
        }


        public async Task<GetProfileUserDtos> VerifyLogin(string? email, string? password)
        {
            if (email.IsNullOrEmpty())
            {
                throw new ModelException(email, "Input Email or phone number is empty");
            }
            if (password.IsNullOrEmpty())
            {
                throw new BadRequestException("Input password is empty");
            }
            var user = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => (u.Email == email) && u.Password!.Equals(password));

            if (user is null)
            {
                throw new NotFoundException("Wrong email, phone number or password");
            }

            return user.Adapt<GetProfileUserDtos>();
        }



        public async Task<GetProfileTutorDtos> RegisterTutor(RegisterTutorDtos registerTutorDtos)
        {
            var userExist = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(us => us.Email == registerTutorDtos.Email);
            if (userExist != null)
            {
                throw new ModelException(userExist.Email, "Email is already existed");
            }

            var fireBaseAuthId = await _fireBaseAuthServices.RegisterUser(registerTutorDtos);
            var mappedUser = registerTutorDtos.Adapt<Models.Models.User>();
            mappedUser.Role = RoleStatus.Tutor;
            mappedUser.Status = UserStatus.Pending;
            mappedUser.FireBaseid = fireBaseAuthId;
            await _unitOfWorkRepository.UserRepository.AddAsync(mappedUser);
            await _unitOfWorkRepository.SaveChangesAsync();

            var rs = mappedUser.Adapt<GetProfileTutorDtos>();
            return rs;
        }

        public async Task<GetProfileUserDtos> GetUserProfileById(int id)
        {



            return (await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Id == id)).Adapt<GetProfileUserDtos>();
        }

        public async Task<bool> DeleteUserAsync(string? email)
        {
            var user = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(ld => ld.Email == email);
            _unitOfWorkRepository.UserRepository.Remove(user);
            return true;
        }

        public async Task<bool> SyncUserAsync(List<ExportedUserRecord> listUserFireData)
        {
            await _unitOfWorkRepository.UserRepository.AddRangeAsync(listUserFireData.Adapt<List<Models.Models.User>>());
            await _unitOfWorkRepository.SaveChangesAsync();
            return true;
        }
    }
}

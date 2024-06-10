using Mapster;
using Microsoft.IdentityModel.Tokens;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.Dtos.Register;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.BusinessLogic.Services.User
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IFireBaseAuthServices _fireBaseAuthServices;

        public UserServices(IUserRepository userRepository, IUnitOfWorkRepository unitOfWorkRepository, IFireBaseAuthServices fireBaseAuthServices)
        {
            _userRepository = userRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
            _fireBaseAuthServices = fireBaseAuthServices;
        }

        public async Task<List<GetProfileUserDtos>> GetAllUsers()
        {
            var userList = await _userRepository.ToListAsync();
            return userList.Adapt<List<GetProfileUserDtos>>();
        }

        public async Task<GetProfileUserDtos> RegisterUser(RegisterDtos registerDtos)
        {
            var userExist = await _userRepository.FirstOrDefaultAsync(us => us.Email == registerDtos.Email);
            if (userExist != null)
            {
                throw new ModelException(userExist.Email, "Email is already existed");
            }

            var fireBaseAuthId = await _fireBaseAuthServices.RegisterUser(registerDtos);

            var mappedUser = registerDtos.Adapt<Models.Models.User>();
            mappedUser.Role = RoleStatus.Customer;
            mappedUser.Uid = fireBaseAuthId;
            var user = await _userRepository.AddAsync(mappedUser);

            await _unitOfWorkRepository.SaveChangesAsync();

            var rs = user.Adapt<GetProfileUserDtos>();
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
            var user = await _userRepository.FirstOrDefaultAsync(u => (u.Email == email) && u.Password!.Equals(password));

            if (user is null)
            {
                throw new NotFoundException("Wrong email, phone number or password");
            }

            return user.Adapt<GetProfileUserDtos>();
        }


    }
}

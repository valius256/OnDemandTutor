using Microsoft.EntityFrameworkCore;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models.Dtos;
using AutoMapper;
using Mapster;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.DataAccess;


namespace OnDemandTutor.BusinessLogic.Services.User
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;   
        public UserServices(IUserRepository userRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _userRepository = userRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }
        public async Task<string> RegisterUser(LoginDto registerDto)
        {
            var userExist = await _userRepository.Where(us => us.Email == registerDto.Email).FirstOrDefaultAsync();
            if (userExist != null)
            {
                throw new ModelException(userExist.Email ,"Email is already existed");
            }

              var mappedUser = registerDto.Adapt<Models.Models.User>(); 
                mappedUser.Role = RoleStatus.Customer;
            var user = await _userRepository.AddAsync(mappedUser);
            _unitOfWorkRepository.SaveChanges();

            return "Success";
            
        }
    }
}

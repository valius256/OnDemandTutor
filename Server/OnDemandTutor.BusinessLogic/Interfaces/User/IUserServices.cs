using OnDemandTutor.Models.Dtos;

namespace OnDemandTutor.BusinessLogic.Interfaces.User
{
    public interface IUserServices
    {
      Task<string> RegisterUser(LoginDto loginDto);
    }
}

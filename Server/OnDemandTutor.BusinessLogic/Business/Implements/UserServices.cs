using OnDemandTutor.BusinessLogic.Business.Interfaces;
using OnDemandTutor.DataAccess.Models;

namespace OnDemandTutor.BusinessLogic.Business.Implements
{
    public class UserServices : IUserService
    {
        public UserServices()
        {

        }

        public Task<User?> GetUserAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}

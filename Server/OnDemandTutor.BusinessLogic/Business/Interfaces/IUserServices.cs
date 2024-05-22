using OnDemandTutor.DataAccess.Models;

namespace OnDemandTutor.BusinessLogic.Business.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserAsync(string id);
    }
}

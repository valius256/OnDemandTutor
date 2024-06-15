using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {

        //Task<IEnumerable<User>> GetQueryAsync(object searchObj);
    }
}

using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<List<User>> GetUsersListDegreeData();
        //Task<IEnumerable<User>> GetQueryAsync(object searchObj);
    }
}

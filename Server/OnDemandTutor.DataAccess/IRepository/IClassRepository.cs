using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        Task<Class?> GetFullDataClass(int id);
    }
}


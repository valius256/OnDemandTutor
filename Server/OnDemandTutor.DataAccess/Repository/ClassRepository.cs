using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        public ClassRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Class?> GetFullDataClass(int id)
        {
            var res = await dbSet.Include(ld => ld.Slots)
                .Where(cl => cl.Id == id)
                .FirstOrDefaultAsync();
            ;
            return res;
        }


    }
}


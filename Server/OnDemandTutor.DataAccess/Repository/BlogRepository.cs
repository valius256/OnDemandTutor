using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}


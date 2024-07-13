using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository
{
    public class TutorVideoRepository : GenericRepository<TutorVideo>, ITutorVideoRepository
    {
        public TutorVideoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
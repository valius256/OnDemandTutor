using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository
{
    public class TutorSubjectRepository : GenericRepository<TutorSubject>, ITutorSubjectRepository
    {
        public TutorSubjectRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
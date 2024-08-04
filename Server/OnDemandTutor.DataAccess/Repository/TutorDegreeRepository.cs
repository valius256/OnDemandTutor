using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository;

public class TutorDegreeRepository : GenericRepository<TutorDegree>, ITutorDegreeRepository
{
    public TutorDegreeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository;

public class SlotStudentRepository : GenericRepository<SlotStudent>, ISlotStudentRepository
{
    public SlotStudentRepository(ApplicationDbContext context) : base(context)
    {
    }
}
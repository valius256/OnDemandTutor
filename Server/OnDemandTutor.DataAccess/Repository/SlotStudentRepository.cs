using Microsoft.EntityFrameworkCore;
using OnDemandTutor.BusinessLogic.Services.Slot;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository;

public class SlotStudentRepository : GenericRepository<SlotStudent>, ISlotStudentRepository
{
    public SlotStudentRepository(ApplicationDbContext context) : base(context)
    {

    }
    public async Task<SlotStudent?> GetClosestFutureSlot(int studentId)
    {
        return await dbSet.AsQueryable()
            .Include(ss => ss.Slot)
                .ThenInclude(s => s.Subject)
            .Include(ss => ss.Slot)
                .ThenInclude(s => s.CreatedBy)
            .Include(ss => ss.Slot)
                .ThenInclude(s => s.Class)
            .Include(ss => ss.User)
            .OrderBy(ss => ss.Slot.StartTime)
            .Where(s => s.Slot.StartTime > DateTime.Now)
            .Take(1)
            .FirstOrDefaultAsync(s => s.User.Id == studentId);
    }
    public async Task<List<SlotStudent>> GetStudentSlotsAsync(QuerySlotStudentDto request, int studentId)
    {
        var query = dbSet.AsQueryable()
            .Include(ss => ss.Slot)
                .ThenInclude(s => s.Subject)
            .Include(ss => ss.Slot)
                .ThenInclude(s => s.CreatedBy)
            .Include(ss => ss.Slot)
                .ThenInclude(s => s.Class)
            .Include(ss => ss.User)
            .Where(s => s.Slot.EndTime <= request.To)
            .Where(s => s.UserId == studentId)
            .Where(s => s.Slot.StartTime >= request.From);

        if (request.PaymentStatus.HasValue)
        {
            query = query.Where(s => s.PaymentStatus == request.PaymentStatus);
        }
        if (request.ClassId.HasValue)
        {
            query = query.Where(s => s.Slot.ClassId == request.ClassId);
        }

        // Apply filtering if necessary

        return await query.ToListAsync();
    }
}
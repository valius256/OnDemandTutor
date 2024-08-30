using Microsoft.EntityFrameworkCore;
using OnDemandTutor.BusinessLogic.Services.Slot;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.StudentSlot;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

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
            .FirstOrDefaultAsync(s => s.User.Id == studentId && s.Slot.EndTime > DateTime.Now && s.Slot.SlotStatus == Models.Enum.SlotStatus.NotYet && s.Slot.RecordStatus != RecordStatus.Deleted);
    }
    public async Task<List<SlotStudent>> GetStudentSlotsAsync(QuerySlotStudentDto request, int? studentId)
    {
        var query = dbSet.AsQueryable()
            .Include(ss => ss.Slot)
                .ThenInclude(s => s.Subject)
            .Include(ss => ss.Slot)
                .ThenInclude(s => s.CreatedBy)
            .Include(ss => ss.Slot)
                .ThenInclude(s => s.Class)
            .Include(ss => ss.User)
            .Where(s => s.Slot.EndTime <= request.To && s.Slot.StartTime >= request.From && s.Slot.RecordStatus != RecordStatus.Deleted);

        if (request.PaymentStatus.HasValue)
        {
            query = query.Where(s => s.PaymentStatus == request.PaymentStatus);
        }
        if (request.ClassId.HasValue)
        {
            query = query.Where(s => s.Slot.ClassId == request.ClassId);
        }
        if (studentId.HasValue)
        {
            query = query.Where(s => s.UserId == studentId);
        }
        if (request.TutorId.HasValue)
        {
            query = query.Where(s => s.Slot.CreateById == request.TutorId);
        }
        // Apply filtering if necessary

        return await query.ToListAsync();
    }

    public async Task<List<SlotStudent>> GetSlotOfStudent(int studentId)
    {
        var query = dbSet.AsQueryable()
            .Include(ss => ss.Slot)
            .Where(ss => ss.UserId == studentId && ss.Slot.RecordStatus != RecordStatus.Deleted);

        return await query.ToListAsync();
    }
    public async Task<PagedResult<SlotStudent>> GetStudentSlotByTutor(PagingModel<QueryRatingDto> queryDto)
    {
        var query = dbSet.Include(s => s.Slot).ThenInclude(s => s.Subject)
            .Include(s => s.User)
             .AsQueryable();

        if (queryDto.Filter != null)
        {
            if (queryDto.Filter.TutorId != 0)
            {
                query = query.Where(s => s.Slot.CreateById == queryDto.Filter.TutorId);
            }
            if (queryDto.Filter.IsRated.HasValue && queryDto.Filter.IsRated.Value)
            {
                query = query.Where(s => (s.Rating != null) == queryDto.Filter.IsRated);
            }
        }

        return await query.Where(s => s.Slot.RecordStatus != RecordStatus.Deleted).ToNewPagingAsync(queryDto.Page > 0 ? queryDto.Page : 1, queryDto.Limit > 0 ? queryDto.Limit : 10);
    }

    public async Task<PagedResult<SlotStudent>> GetStudentsSlotWithStudentBySlotIdPaged(int slotId, int page, int limit)
    {
        var query = dbSet.Include(s => s.User)
             .Where(s => s.SlotId == slotId);

        return await query.ToNewPagingAsync(page > 0 ? page : 1, limit > 0 ? limit : 10);
    }
    public async Task<List<SlotStudent>> GetStudentsSlotWithStudentBySlotId(int slotId)
    {
        var query = dbSet.Include(s => s.User)
             .Where(s => s.SlotId == slotId);

        return await query.ToListAsync();
    }

    public async Task<List<SlotStudent>> GetAboutToStartStudentSlots()
    {
        var query = dbSet.AsQueryable()
            .Include(ss => ss.Slot).ThenInclude(s => s.CreatedBy)
            .Include(ss => ss.User)
            .Where(ss => ss.Slot.RecordStatus != RecordStatus.Deleted && ss.Slot.StartTime <= DateTime.Now.AddHours(1) && ss.PaymentStatus == Models.Enum.PaymentStatus.Notpaid);

        return await query.ToListAsync();
    }

    public async Task<SlotStudent?> GetSlotStudentBySlotIdAndStudentId(int slotId, int studentId)
    {
        return await dbSet.AsQueryable()
            .Include(ss => ss.Slot)
            .Include(ss => ss.User)
            .FirstOrDefaultAsync(ss => ss.Slot.RecordStatus != RecordStatus.Deleted && ss.UserId == studentId && ss.SlotId == slotId);
    }
}
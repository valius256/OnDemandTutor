using Mapster;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository
{
    public class SlotRepository : GenericRepository<Slot>, ISlotRepository
    {
        public SlotRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<GetSlotsDtos>> GetSlotsAsync(PagingModel<QuerySlotDto> request)
        {
            var query = dbSet.Include(s => s.CreatedBy).Include(s => s.Subject).Include(s => s.Class).AsQueryable();
            if (request.Filter != null)
            {
                if (request.Filter.ClassId.HasValue)
                {
                    query = query.Where(s => s.ClassId == request.Filter.ClassId);
                }
                if (request.Filter.UserId.HasValue)
                {
                    query = query.Where(s => s.CreateById == request.Filter.UserId);
                }
                if (request.Filter.SubjectId.HasValue)
                {
                    query = query.Where(s => s.SubjectId == request.Filter.SubjectId);
                }
                if (request.Filter.Start.HasValue)
                {
                    query = query.Where(s => s.StartTime >= request.Filter.Start);
                }
                if (request.Filter.End.HasValue)
                {
                    query = query.Where(s => s.EndTime <= request.Filter.End);
                }
            }
            // Apply filtering if necessary

            var results = await query.ToNewPagingAsync(request.Page, request.Limit);
            return results.Adapt<PagedResult<GetSlotsDtos>>();
        }
        public async Task<GetSlotDetailDto> GetSlotByIdAsync(int id)
        {
            var slot = await dbSet
                .Include(s => s.CreatedBy).Include(s => s.Subject).Include(s => s.Class)
                .Include(s => s.SlotStudents)
                .FirstOrDefaultAsync(s => s.Id == id);

            return slot.Adapt<GetSlotDetailDto>();
        }
        public async Task<bool> DeleteSlotAsync(int id)
        {
            var slot = await dbSet.FindAsync(id);
            if (slot == null)
            {
                return false; // Slot not found
            }

            dbSet.Remove(slot);
            await context.SaveChangesAsync();
            return true;
        }
        //public async Task<List<GetSlotWithSlotStudentDto>> GetSlotWithSlotStudentByStudentId(int studentId)
        //{
        //    var listSlot = await dbSet
        //        .Include(ld => ld.SlotStudents)
        //        .Where(ld => ld.SlotStudents.Any(ss => ss.UserId == studentId))
        //        .ToListAsync();


        //    return listSlot.Adapt<List<GetSlotWithSlotStudentDto>>();
        //}

        public async Task<Slot?> GetClosestFutureSlotOfTutor(int tutorId)
        {
            return await dbSet.AsQueryable()
                .Include(s => s.Subject)
                .Include(s => s.CreatedBy)
                .Include(s => s.Class)
                .OrderBy(s => s.StartTime)
                .Where(s => s.EndTime > DateTime.Now)
                .Take(1)
                .FirstOrDefaultAsync(s => s.CreateById == tutorId);
        }

    }
}


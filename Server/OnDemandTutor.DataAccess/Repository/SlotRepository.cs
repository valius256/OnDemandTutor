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
            var query = dbSet.AsQueryable();
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

            var results = await query.ToNewPagingAsync<Slot>(request.Page, request.Limit);
            return results.Adapt<PagedResult<GetSlotsDtos>>();
        }
        public async Task<GetSlotsDtos> GetSlotByIdAsync(int id)
        {
            var slot = await dbSet.FindAsync(id);
            if (slot == null)
                return null;

            return slot.Adapt<GetSlotsDtos>();
        }
        public async Task<CreateSlotsDto> CreateSlotAsync(CreateSlotsDto slotDto)
        {
            var slot = slotDto.Adapt<Slot>();

            await dbSet.AddAsync(slot);
            await context.SaveChangesAsync();

            return slotDto;
        }
        public async Task<UpdateSlotDto> UpdateSlotAsync(UpdateSlotDto slotDto)
        {
            var existingSlot = await dbSet.FindAsync(slotDto.Id);
            if (existingSlot == null)
            {
                throw new Exception("Slot not found");
            }

            existingSlot.StartTime = slotDto.StartTime;
            existingSlot.EndTime = slotDto.EndTime;
            existingSlot.TeachAddress = slotDto.TeachAddress;
            existingSlot.ClassId = slotDto.ClassId;
            existingSlot.SubjectId = slotDto.SubjectId;
            existingSlot.IsOnline = slotDto.IsOnline;
            existingSlot.NumberOfStudents = slotDto.NumberOfStudents;
            existingSlot.SlotStudents.FirstOrDefault().PaymentStatus = slotDto.PaymentStatus;
            existingSlot.ActualEndTime = slotDto.ActualEndTime;
            // Update other properties as needed

            context.Entry(existingSlot).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return slotDto;
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


        public async Task<GetSlotWithSlotStudentDto?> GetSlotWithSlotStudentStudentById(int id)
        {
            var slot = await dbSet
                .Include(ld => ld.SlotStudents)
                .FirstOrDefaultAsync(ld => ld.Id == id);

            return slot?.Adapt<GetSlotWithSlotStudentDto>();
        }

        public async Task<List<GetSlotWithSlotStudentDto>?> GetSlotWithSlotStudentByStudentId(int studentId)
        {
            var listSlot = await dbSet
                .Include(ld => ld.SlotStudents)
                .Where(ld => ld.SlotStudents.Any(ss => ss.UserId == studentId))
                .ToListAsync();


            return listSlot?.Adapt<List<GetSlotWithSlotStudentDto>>();
        }
    }
}


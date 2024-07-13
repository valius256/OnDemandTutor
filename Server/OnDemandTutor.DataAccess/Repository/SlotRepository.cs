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

        public async Task<PagedResult<GetSlotsDtos>> GetSlotsAsync(PagingModel<GetSlotsDtos> request)
        {
            var query = dbSet.AsQueryable();
            // Apply filtering if necessary
            if (request.Filter != null)
            {
                if (request.Filter.SubjectId.HasValue)
                {
                    query = dbSet.Where(slot => slot.SubjectId == request.Filter.SubjectId);
                }
                if (request.Filter.ClassId.HasValue)
                {
                    query = query.Where(slot => slot.ClassId == request.Filter.ClassId);
                }
                // Add other filters based on the properties of GetSlotsDtos
            }
            var results = await dbSet.ToPagingAsync<GetSlotsDtos, Slot>(request.Page, request.Limit);
            return results;
        }
        public async Task<GetSlotsDtos> GetSlotByIdAsync(int id)
        {
            var slot = await dbSet.FindAsync(id);
            if (slot == null)
                return null;

            return slot.Adapt<GetSlotsDtos>();
        }
        public async Task<CreateSlotsDtos> CreateSlotAsync(CreateSlotsDtos slotDto)
        {
            var slot = slotDto.Adapt<Slot>();

            await dbSet.AddAsync(slot);
            await context.SaveChangesAsync();

            return slotDto;
        }
        public async Task<UpdateSlotDtos> UpdateSlotAsync(UpdateSlotDtos slotDto)
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


        public async Task<GetSlotWithSlotStudentDto?> GetSlotWithStudentById(int id)
        {
            var slot = await dbSet
                .Include(ld => ld.SlotStudents)
                .FirstOrDefaultAsync(ld => ld.Id == id);

            return slot?.Adapt<GetSlotWithSlotStudentDto>();
        }

    }
}


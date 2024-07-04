using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.Slot;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.Slot
{
    public class SlotService : ISlotServices
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly ISlotRepository _slotRepository;

        public SlotService(IUnitOfWorkRepository unitOfWorkRepository, ISlotRepository slotRepository)
        {
            _unitOfWork = unitOfWorkRepository;
            _slotRepository = slotRepository;
        }


        public async Task<PagedResult<GetSlotsDtos>> GetSlotsAsync(PagingModel<GetSlotsDtos> request)
        {
            return await _unitOfWork.SlotRepository.GetSlotsAsync(request);
        }

        public async Task<GetSlotsDtos> GetSlotByIdAsync(int id)
        {
            var slot = await _unitOfWork.SlotRepository.GetSlotByIdAsync(id);
            if (slot is null)
            {
                throw new BadRequestException("Slot not found");
            }
            return slot;
        }

        public async Task<CreateSlotsDtos> CreateSlotAsync(CreateSlotsDtos slotDto)
        {
            var existedSlotID = await _unitOfWork.SlotRepository.FirstOrDefaultAsync(s => s.Id == slotDto.Id);
            if (existedSlotID != null)
            {
                throw new ModelException("Slot", $"{existedSlotID.Id},already exited, please Try again", "The id is exsited");
            }
            var slotEntity = slotDto.Adapt<CreateSlotsDtos>(); // Assuming Mapster is used for mapping

            // Add the new Slot entity to repository
            var createdSlotEntity = await _unitOfWork.SlotRepository.CreateSlotAsync(slotEntity);
            await _unitOfWork.SaveChangesAsync();

            // Map the created entity back to CreateSlotsDtos and return it
            var createdSlotDto = createdSlotEntity.Adapt<CreateSlotsDtos>(); // Mapster mapping

            return createdSlotDto;
        }
        public async Task<UpdateSlotDtos> UpdateSlotAsync(UpdateSlotDtos slotDto)
        {
            var slot = slotDto.Adapt<UpdateSlotDtos>();
            if (slot == null)
            {
                throw new NotFoundException($"Slot with ID {slotDto.Id} not found.");
            }
            var updatedSlot = await _unitOfWork.SlotRepository.UpdateSlotAsync(slot);
            await _unitOfWork.SaveChangesAsync();
            return updatedSlot.Adapt<UpdateSlotDtos>();
        }

        public async Task<bool> DeleteSlotAsync(int id)
        {
            var isDeleted = await _unitOfWork.SlotRepository.DeleteSlotAsync(id);
            await _unitOfWork.SaveChangesAsync();
            if (!isDeleted)
            {
                throw new NotFoundException($"Slot with ID {id} not found.");
            }
            return isDeleted;
        }

    }
}


using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.Class;
using OnDemandTutor.BusinessLogic.Interfaces.Slot;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Interfaces.StudentClass;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.Class
{
    public class ClassServices : IClassServices
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
    
        // private readonly ISlotServices _slotServices;

        public ClassServices(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
            // _slotServices = slotServices;
        }

        public async Task<PagedResult<GetClassDtos>> GetClasses(PagingModel<QueryClassDTO> request)
        {
            var pagedResult = await _unitOfWork.ClassRepository.GetClasses(request);
            var mappedResult = pagedResult.Adapt<PagedResult<GetClassDtos>>();
            foreach (var result in mappedResult.Items)
            {
                var class_ = pagedResult.Items.FirstOrDefault(x => x.Id == result.Id);
                var classSlots = class_?.Slots.ToList() ?? new List<Models.Models.Slot>();
                if( classSlots.Any())
                {
                result.StartTime = classSlots[0].StartTime;
                    result.EndTime = classSlots[classSlots.Count-1].EndTime;
            }
            }
            return mappedResult;
        }
        public async Task<PagedResult<GetClassDtos>> GetClassesOfStudent(int studentId, int page, int limit)
        {
            var pagedResult = await _unitOfWork.ClassRepository.GetClassesOfStudent(studentId,page,limit);
            var mappedResult = pagedResult.Adapt<PagedResult<GetClassDtos>>();
            foreach (var result in mappedResult.Items)
            {
                var class_ = pagedResult.Items.FirstOrDefault(x => x.Id == result.Id);
                var classSlots = class_?.Slots.ToList() ?? new List<Models.Models.Slot>();
                if (classSlots.Any())
                {
                    result.StartTime = classSlots[0].StartTime;
                    result.EndTime = classSlots[classSlots.Count - 1].EndTime;
                }
            }
            return mappedResult;
        }
        public async Task<PagedResult<GetClassDtos>> GetClassesOfTutor(int studentId, int page, int limit)
        {
            var pagedResult = await _unitOfWork.ClassRepository.GetClassesOfTutor(studentId, page, limit);
            var mappedResult = pagedResult.Adapt<PagedResult<GetClassDtos>>();
            foreach (var result in mappedResult.Items)
            {
                var class_ = pagedResult.Items.FirstOrDefault(x => x.Id == result.Id);
                var classSlots = class_?.Slots.ToList() ?? new List<Models.Models.Slot>();
                if (classSlots.Any())
        {
                    result.StartTime = classSlots[0].StartTime;
                    result.EndTime = classSlots[classSlots.Count - 1].EndTime;
                }
            }
            return mappedResult;
        }
        public async Task<GetClassFullDataSlotDto> GetClassByIdAsync(int id)
        {
            var classEntity = await _unitOfWork.ClassRepository.GetClassWithSlotsByIdAsync(id);

            if (classEntity is null)
            {
                throw new Exception("Class not found");
            }
            var rs = classEntity.Adapt<GetClassFullDataSlotDto>();

            if( rs is not null)
            {
                rs.StartTime = classEntity.Slots.First().StartTime;
                rs.EndTime = classEntity.Slots.Last().EndTime;
            }
           
            return rs;
        }

        public async Task<CreateClassDTO> CreateClassAsync(CreateClassDTO classDto)
        {
            var classEntity = classDto.Adapt<Models.Models.Class>();
            var createdClass = await _unitOfWork.ClassRepository.AddAsync(classEntity);
            var rs = createdClass.Entity.Adapt<CreateClassDTO>();
            foreach (var slotId in classDto.SlotIds)
            {
                var slot = await _unitOfWork.SlotRepository.GetSlotByIdAsync(slotId);
                if (slot != null)
                {
                    var mapper = slot.Adapt<Models.Models.Slot>();
                    slot.ClassId = createdClass.Entity.Id;

                    _unitOfWork.SlotRepository.Update(mapper);
                }
            }
            await _unitOfWork.SaveChangesAsync();
            return rs;
        }

        public async Task<GetClassDtos> UpdateClassAsync(GetClassDtos classDto)
        {
            var classEntity = classDto.Adapt<Models.Models.Class>();
            var updatedClass = _unitOfWork.ClassRepository.Update(classEntity);
            await _unitOfWork.SaveChangesAsync();
            return updatedClass.Entity.Adapt<GetClassDtos>();
        }

        public async Task<bool> DeleteClassAsync(int id)
        {
            var classEntity = await _unitOfWork.ClassRepository.FirstOrDefaultAsync(c => c.Id == id);
            if (classEntity == null)
            {
                throw new Exception("Class not found");
            }
            _unitOfWork.ClassRepository.Remove(classEntity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<GetClassFullDataSlotDto> GetClassWithFullDataSlotId(int id)
        {
            var classWithSlot = await _unitOfWork.ClassRepository.GetFullDataClass(id);
            return classWithSlot.Adapt<GetClassFullDataSlotDto>();
        }

        public async Task CronForAutoChangeStatusClassAndSlot()
        {
            var classesToUpdate = await _unitOfWork.ClassRepository
                .WhereAsync(cl => cl.Status == ClassStatus.NotStart ||
                                  cl.Status == ClassStatus.OnGoing);

            foreach (var classModel in classesToUpdate)
            {
                // change for slot first 
                bool allSlotsFinished = true;
                foreach (var slot in classModel.Slots.ToList())
                {
                    if (slot.StartTime <= DateTime.Now && slot.SlotStatus == SlotStatus.NotYet)
                    {
                        slot.SlotStatus = SlotStatus.OnGoing;
                    }

                    if (slot.EndTime <= DateTime.Now && slot.SlotStatus == SlotStatus.OnGoing)
                    {
                        slot.SlotStatus = SlotStatus.Finished;
                    }

                    if (slot.SlotStatus != SlotStatus.Finished)
                    {
                        allSlotsFinished = false;
                    }

                    var slotStatusUpdateModel = slot.Adapt<Models.Models.Slot>(); 
                    _unitOfWork.SlotRepository.Update(slotStatusUpdateModel);
                    await _unitOfWork.SaveChangesAsync();
                }
    
                
                // when all slot reach to finished , change class status to finished
                if (classModel.Status == ClassStatus.NotStart && classModel.Slots.Any(sl => sl.SlotStatus == SlotStatus.OnGoing))
                {
                    classModel.Status = ClassStatus.OnGoing;
                }

                if (allSlotsFinished && classModel.Status == ClassStatus.OnGoing)
                {
                    classModel.Status = ClassStatus.Finished;
                }

                _unitOfWork.ClassRepository.Update(classModel);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}


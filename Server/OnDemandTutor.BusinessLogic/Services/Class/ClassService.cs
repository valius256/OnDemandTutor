using LinqKit;
using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.Class;
using OnDemandTutor.DataAccess;
using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.Class
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public ClassService(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<GetClassDtos>> GetClassesAsync(PagingModel<GetClassDtos> pagingModel)
        {
            var pagedResult = await _unitOfWork.ClassRepository.PagingAsync(pagingModel.Adapt<PagingModel<Models.Models.Class>>());
            var mappedResult = pagedResult.Adapt<PagedResult<GetClassDtos>>();
            foreach (var result in mappedResult.Items)
            {
                var class_ = pagedResult.Items.FirstOrDefault(x => x.Id == result.Id);
                if (class_ != null && class_.Slots.Any())
                {
                    var classSlots = class_.Slots.OrderBy(s => s.StartTime).ToList();
                    result.StartTime = classSlots.First().StartTime;
                    result.EndTime = classSlots.Last().EndTime;
                }

            }
            return mappedResult;
        }
        public async Task<PagedResult<GetClassDtos>> GetClasses(PagingModel<QueryClassDTO> request)
        {
            var classPagedResult = await _unitOfWork.ClassRepository.GetClasses(request);
            //var classDtos = _mapper.Map<PagedResult<GetClassDtos>>(classPagedResult);
            var classDtos = classPagedResult.Adapt<PagedResult<GetClassDtos>>();
            return classDtos;
        }


        public async Task<GetClassFullDataSlotDto> GetClassByIdAsync(int id)
        {
            var classEntity = await _unitOfWork.ClassRepository.FirstOrDefaultAsync(c => c.Id == id);
            if (classEntity == null)
            {
                throw new Exception("Class not found");
            }
            var rs = classEntity.Adapt<GetClassFullDataSlotDto>();
            if(rs is not null)
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

    }
}


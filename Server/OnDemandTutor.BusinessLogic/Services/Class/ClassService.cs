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
            var dtoPagedResult = new PagedResult<GetClassDtos>
            {
                Items = pagedResult.Items.Adapt<List<GetClassDtos>>(),
                Limit = pagedResult.Limit,
                Page = pagedResult.Page,
                Total = pagedResult.Total,
            };
            return dtoPagedResult;
        }

        public async Task<GetClassDtos> GetClassByIdAsync(int id)
        {
            var classEntity = await _unitOfWork.ClassRepository.FirstOrDefaultAsync(c => c.Id == id);
            if (classEntity == null)
            {
                throw new Exception("Class not found");
            }
            return classEntity.Adapt<GetClassDtos>();
        }

        public async Task<GetClassDtos> CreateClassAsync(GetClassDtos classDto)
        {
            var classEntity = classDto.Adapt<Models.Models.Class>();
            var createdClass = await _unitOfWork.ClassRepository.AddAsync(classEntity);
            await _unitOfWork.SaveChangesAsync();
            return createdClass.Entity.Adapt<GetClassDtos>();
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
    }
}


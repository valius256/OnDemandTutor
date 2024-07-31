using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Class
{
    public interface IClassServices
    {

        Task<PagedResult<GetClassDtos>> GetClasses(PagingModel<QueryClassDTO> pagingModel);
        Task<PagedResult<GetClassDtos>> GetClassesOfStudent(int studentId, int page, int limit);
        Task<PagedResult<GetClassDtos>> GetClassesOfTutor(int studentId, int page, int limit);
        Task<GetClassFullDataSlotDto> GetClassByIdAsync(int id);
        Task<CreateClassDTO> CreateClassAsync(CreateClassDTO classDto);
        Task<GetClassDtos> UpdateClassAsync(GetClassDtos classDto);
        Task<bool> DeleteClassAsync(int id);
        Task CronForAutoChangeStatusClassAndSlot();
        Task<bool> EnrollCLass(int classId, int studentId);

        Task ValidateClassForStudent(int classId, int studentId);
    }
}


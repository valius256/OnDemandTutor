using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Class
{
    public interface IClassServices
    {

        Task<PagedResult<GetClassDtos>> GetClasses(PagingModel<QueryClassDTO> pagingModel);
        Task<PagedResult<GetClassDtos>> GetClassesOfStudent(int studentId, int page, int limit);
        Task<PagedResult<GetClassDtos>> GetClassesOfTutor(int studentId, int page, int limit);
        Task<GetClassFullDataSlotDto> GetClassByIdAsync(int id);
        Task<GetClassDtos> CreateClassAsync(CreateClassDTO classDto, GetProfileUserDtos user);
        Task<GetClassDtos> UpdateClassAsync(GetClassDtos classDto);
        Task<bool> DeleteClassAsync(int id);
        Task CronForAutoChangeStatusClassAndSlot();
        Task<List<Models.Models.StudentClass>> GetAllStudentInClassWithClassId(int classId);
        Task ValidateClassForStudent(int classId, int studentId);
    }
}


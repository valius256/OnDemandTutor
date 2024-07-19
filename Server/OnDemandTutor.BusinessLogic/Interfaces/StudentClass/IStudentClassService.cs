using OnDemandTutor.Models.Dtos.StudentClass;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.StudentClass
{
    public interface IStudentClassService
    {
        Task<PagedResult<GetStudentClassDto>> GetStudentClassesAsync(PagingModel<GetStudentClassDto> pagingModel);
        Task<GetStudentClassDto> GetStudentClassByIdAsync(int id);
        Task<CreateStudentClassDto> CreateStudentClassAsync(CreateStudentClassDto studentClassDto);
        Task<UpdateStudentClassDto> UpdateStudentClassAsync(UpdateStudentClassDto studentClassDto);
        Task<bool> DeleteStudentFromStudentClassById(int classId, int userId);
        Task<bool> DeleteStudentClassAsync(int id);
        Task<bool> StudentRatingClassAsync(int classId, int studentId, int Rating, string? Feedback);
        Task<Models.Models.StudentClass> CreateStudentClassIfNotExist(int classId, int studentId);

    }
}





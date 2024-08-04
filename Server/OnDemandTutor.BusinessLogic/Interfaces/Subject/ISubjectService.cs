using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Subject;

public interface ISubjectService
{
    Task<PagedResult<GetSubjectDtos>> GetSubjectsAsync(PagingModel<QuerySubjectDTO> request);
    Task<GetSubjectDtos> GetSubjectByIdAsync(int id);
    Task<GetSubjectDtos> CreateSubjectAsync(CreateSubjectDtos subjectCreateDto);
    Task<GetSubjectDtos> UpdateSubjectAsync(UpdateSubjectDtos subjectGetDto);
    Task<bool> DeleteSubjectAsync(int id);
}
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Subject
{
    public interface ISubjectService
    {
        Task<PagedResult<GetSubjectDtos>> GetSubjectsAsync(PagingModel<GetSubjectDtos> request);
        Task<GetSubjectDtos> GetSubjectByIdAsync(int id);
        Task<CreateSubjectDtos> CreateSubjectAsync(CreateSubjectDtos subjectCreateDto);
        Task<GetSubjectDtos> UpdateSubjectAsync(GetSubjectDtos subjectGetDto);
        Task<bool> DeleteSubjectAsync(int id);

    }
}


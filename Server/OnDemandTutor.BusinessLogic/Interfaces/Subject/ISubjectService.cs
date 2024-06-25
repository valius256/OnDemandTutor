using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Subject
{
    public interface ISubjectService
    {
        Task<List<GetSubjectDtos>> GetAllSubjects();
        Task<GetSubjectDtos> GetSubjectById(int id);
        Task<CreateSubjectDtos> CreateSubject(CreateSubjectDtos subjectDto);
        Task<UpdateSubjectDtos> UpdateSubject(UpdateSubjectDtos subjectDto);
        Task<bool> DeleteSubject(int id);
        Task<List<GetSubjectDtos>> SearchSubjectsByName(string name);
        Task<PagedResult<GetSubjectDtos>> GetSubjects(PagingModel<GetSubjectDtos> pagingModel);

    }
}


using OnDemandTutor.Models.Dtos.TutorSubject;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.TutorSubject
{
    public interface ITutorSubjectService
    {
        Task<PagedResult<GetTutorSubjectDto>> GetTutorSubjectsAsync(PagingModel<GetTutorSubjectDto> request);
        Task<GetTutorSubjectDto> GetTutorSubjectByIdAsync(int id);
        Task<CreateTutorSubjectDto> CreateTutorSubjectAsync(CreateTutorSubjectDto tutorSubjectDto);
        Task<UpdateTutorSubjectDto> UpdateTutorSubjectAsync(UpdateTutorSubjectDto tutorSubjectDto);
        Task<bool> DeleteTutorSubjectAsync(int id);
    }
}
using OnDemandTutor.Models.Dtos.TutorSubject;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.TutorSubject
{
    public interface ITutorSubjectService
    {
        Task<PagedResult<GetTutorSubjectWithUserAndSubjectDto>> GetTutorSubjectsAsync(PagingModel<QueryTutorSubjectDto> request);
        Task<GetTutorSubjectDetailDto> GetTutorSubjectByIdAsync(int id);
        Task<GetTutorSubjectDetailDto> CreateTutorSubjectAsync(CreateTutorSubjectDto tutorSubjectDto, GetProfileUserDtos user);
        Task<UpdateTutorSubjectDto> UpdateTutorSubjectAsync(UpdateTutorSubjectDto tutorSubjectDto);
        Task<bool> DeleteTutorSubjectAsync(int id);
    }
}
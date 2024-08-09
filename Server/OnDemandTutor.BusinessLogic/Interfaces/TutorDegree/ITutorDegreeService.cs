using OnDemandTutor.Models.Dtos.TutorDegree;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.TutorDegree
{
    public interface ITutorDegreeService
    {
        Task<PagedResult<GetTutorDegreeDto>> GetTutorDegreesAsync(PagingModel<GetTutorDegreeDto> request);
        Task<GetTutorDegreeDto> GetTutorDegreeByIdAsync(int id);
        Task<List<GetTutorDegreeDto>> GetTutorDegreesByTutorIdAndSubjectId(int tutorId, int subjectId);
        Task<GetTutorDegreeDto> CreateTutorDegreeAsync(CreateTutorDegreeDto tutorDegreeDto);
        Task UpsertTutorDegreeAsync(List<UpdateTutorDegreeDto> newDegreeDtos, List<GetTutorDegreeDto> oldTutorDegreeDtos, int userId, int subjectId);
        Task<bool> DeleteTutorDegreeAsync(int id);
    }
}
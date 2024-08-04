using OnDemandTutor.Models.Dtos.TutorSubject;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository;

public interface ITutorSubjectRepository : IGenericRepository<TutorSubject>
{
    Task<TutorSubject?> GetTutorSubjectById(int id);
    Task<PagedResult<TutorSubject>> GetTutorSubjects(PagingModel<QueryTutorSubjectDto> request);
}
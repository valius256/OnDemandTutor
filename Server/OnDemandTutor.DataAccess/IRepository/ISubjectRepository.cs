using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.RequestModel.Subject;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {

        Task<bool> CheckSubjectExists(string subjectName);
        Task<GetSubjectDtos> GetSubjectByCode(int code);
        Task<GetSubjectDtos> GetSubjectByName(string name);
        Task<IEnumerable<GetSubjectDtos>> GetSubjectsByCategory(string category);
        Task<bool> IsSubjectActive(int subjectId);
        Task<IEnumerable<GetSubjectDtos>> SearchSubjectsByName(string name);
        Task UpdateSubjectDescription(SubjectRequestModel request);


    }
}


using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.RequestModel.Subject;

namespace OnDemandTutor.BusinessLogic.Interfaces;

public interface ISubjectService
{
    Task<bool> CheckSubjectExists(string subjectName);
    Task<GetSubjectDtos> GetSubjectByCode(int code);
    Task<GetSubjectDtos> GetSubjectByName(string name);
    Task<IEnumerable<GetSubjectDtos>> GetSubjectsByCategory(string category);
    Task<bool> IsSubjectActive(int subjectId);
    Task<IEnumerable<GetSubjectDtos>> SearchSubjectsByName(string name);
    Task UpdateSubjectDescription(SubjectRequestModel requset);
}
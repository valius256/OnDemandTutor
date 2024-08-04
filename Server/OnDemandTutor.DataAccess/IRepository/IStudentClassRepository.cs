using OnDemandTutor.Models.Dtos.StudentClass;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository;

public interface IStudentClassRepository : IGenericRepository<StudentClass>
{
    Task<PagedResult<StudentClass>> QueryStudentClass(PagingModel<QueryStudentClassDto> request);
}
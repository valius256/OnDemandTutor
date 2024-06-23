using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository;

public interface IUserRepository : IGenericRepository<User>
{
    Task<List<User>> GetUsersListDegreeData();
    //Task<IEnumerable<User>> GetQueryAsync(object searchObj);
    Task<User> GetTutorRegistration(string firebaseId);
    Task<PagedResult<TutorSimpleProfileDtos>> GetTutorListAsync(PagingModel<TutorSimpleProfileRequest> request);
}
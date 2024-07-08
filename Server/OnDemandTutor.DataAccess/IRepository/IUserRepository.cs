using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository;

public interface IUserRepository : IGenericRepository<User>
{
    Task<PagedResult<User>> ViewUsersListAsync(UserFilterDto request);
    Task<List<User>> GetUsersListDegreeData();
    Task<List<TutorRegistrationResponseDtos>> GetTutorRegistration(string firebaseId);
    Task<PagedResult<TutorSimpleProfileDto>> ViewTutorListAsync(TutorFilterDto request);
    Task<PagedResult<GetOutstandingTutorDto>> GetOutStandingTutors(int limit, int page);

}
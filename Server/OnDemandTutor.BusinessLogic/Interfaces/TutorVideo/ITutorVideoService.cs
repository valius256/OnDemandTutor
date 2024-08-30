
using OnDemandTutor.Models.Dtos.TutorVideo;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.TutorVideo
{
    public interface ITutorVideoService
    {
        Task<PagedResult<GetTutorVideoDto>> GetTutorVideosAsync(PagingModel<QueryTutorVideoDto> request);
        Task<GetTutorVideoDto> GetTutorVideoByIdAsync(int id);
        Task<GetTutorVideoDto> CreateTutorVideoAsync(CreateTutorVideoDto tutorVideoDto, GetProfileUserDto user);
        Task<GetTutorVideoDto> UpdateTutorVideoAsync(UpdateTutorVideoDto tutorVideoDto, GetProfileUserDto user);
        Task<bool> DeleteTutorVideoAsync(int id);
    }
}
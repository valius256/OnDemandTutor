
using OnDemandTutor.Models.Dtos.TutorVideo;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.TutorVideo
{
    public interface ITutorVideoService
    {
        Task<PagedResult<GetTutorVideoDto>> GetTutorVideosAsync(PagingModel<GetTutorVideoDto> request);
        Task<GetTutorVideoDto> GetTutorVideoByIdAsync(int id);
        Task<CreateTutorVideoDto> CreateTutorVideoAsync(CreateTutorVideoDto tutorVideoDto);
        Task<UpdateTutorVideoDto> UpdateTutorVideoAsync(UpdateTutorVideoDto tutorVideoDto);
        Task<bool> DeleteTutorVideoAsync(int id);
    }
}
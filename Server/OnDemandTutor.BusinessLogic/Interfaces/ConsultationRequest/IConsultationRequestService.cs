using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Paging;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Interfaces
{
    public interface IConsultationRequestService
    {
        Task<PagedResult<GetConsultationRequestDto>> GetConsultationRequestsAsync(PagingModel<GetConsultationRequestDto> pagingModel);
        Task<GetConsultationRequestDto> GetConsultationRequestByIdAsync(int id);
        Task<GetConsultationRequestDto> CreateConsultationRequestAsync(RegisterConsultationRequestDto consultationRequestDto);
        Task<GetConsultationRequestDto> UpdateConsultationRequestAsync(GetConsultationRequestDto consultationRequestDto);
        Task<bool> DeleteConsultationRequestAsync(int id);
        Task<bool> HandleConsultationRequestAsync(ClaimsPrincipal claimsPrincipal, HandleConsultationRequestDto requestDtos);
        Task<List<GetConsultationRequestDto>> ViewAllConsultationsRequestAsync(ConsultationRequestFilterDto request);
    }
}


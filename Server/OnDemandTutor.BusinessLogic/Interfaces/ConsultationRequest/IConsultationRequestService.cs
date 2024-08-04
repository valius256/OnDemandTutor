using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces;

public interface IConsultationRequestService
{
    Task<PagedResult<GetConsultationRequestDto>> GetConsultationRequestsAsync(
        PagingModel<GetConsultationRequestDto> pagingModel);

    Task<GetConsultationRequestDto> GetConsultationRequestByIdAsync(int id);

    Task<GetConsultationRequestDto> CreateConsultationRequestAsync(
        RegisterConsultationRequestDto consultationRequestDto);

    Task<GetConsultationRequestDto> UpdateConsultationRequestAsync(GetConsultationRequestDto consultationRequestDto);
    Task<bool> DeleteConsultationRequestAsync(int id);
    Task<bool> HandleConsultationRequestAsync(GetProfileUserDtos user, HandleConsultationRequestDto requestDtos);
    Task<PagedResult<GetConsultationRequestDto>> ViewAllConsultationsRequestAsync(ConsultationRequestFilterDto request);
}
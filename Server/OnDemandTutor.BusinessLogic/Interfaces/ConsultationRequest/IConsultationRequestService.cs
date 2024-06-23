using System;
using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces
{
    public interface IConsultationRequestService
    {
        Task<PagedResult<GetConsultationRequestDtos>> GetConsultationRequestsAsync(PagingModel<GetConsultationRequestDtos> pagingModel);
        Task<GetConsultationRequestDtos> GetConsultationRequestByIdAsync(int id);
        Task<GetConsultationRequestDtos> CreateConsultationRequestAsync(GetConsultationRequestDtos consultationRequestDto);
        Task<GetConsultationRequestDtos> UpdateConsultationRequestAsync(GetConsultationRequestDtos consultationRequestDto);
        Task<bool> DeleteConsultationRequestAsync(int id);
    }
}


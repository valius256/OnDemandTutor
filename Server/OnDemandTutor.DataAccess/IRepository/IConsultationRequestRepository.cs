using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface IConsultationRequestRepository : IGenericRepository<ConsultationRequest>
    {
        Task<PagedResult<ConsultationRequest>> ViewAllConsultationsRequestAsync(ConsultationRequestFilterDto request);
    }
}


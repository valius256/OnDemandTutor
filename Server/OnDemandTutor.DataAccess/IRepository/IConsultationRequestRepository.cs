using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface IConsultationRequestRepository : IGenericRepository<ConsultationRequest>
    {
        Task<List<GetConsultationRequestDto>> ViewAllConsultationsRequestAsync(ConsultationRequestFilterDto request);
    }
}


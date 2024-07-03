using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.DataAccess;
using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Paging;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Services.ConsultationRequest
{
    public class ConsultationRequestService : IConsultationRequestService
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ConsultationRequestService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<PagedResult<GetConsultationRequestDto>> GetConsultationRequestsAsync(PagingModel<GetConsultationRequestDto> pagingModel)
        {
            var pagedResult = await _unitOfWorkRepository.ConsultationRequestRepository.PagingAsync(pagingModel.Adapt<PagingModel<Models.Models.ConsultationRequest>>());
            var dtoPagedResult = new PagedResult<GetConsultationRequestDto>
            {
                Items = pagedResult.Items.Adapt<List<GetConsultationRequestDto>>(),
                Page = pagedResult.Page,
                Limit = pagedResult.Limit,
                Total = pagedResult.Total
            };
            return dtoPagedResult;
        }

        public async Task<GetConsultationRequestDto> GetConsultationRequestByIdAsync(int id)
        {
            var consultationRequest = await _unitOfWorkRepository.ConsultationRequestRepository.FirstOrDefaultAsync(c => c.Id == id);
            return consultationRequest?.Adapt<GetConsultationRequestDto>();
        }

        public async Task<GetConsultationRequestDto> CreateConsultationRequestAsync(RegisterConsultationRequestDto consultationRequestDto)
        {
            var consultationRequest = consultationRequestDto.Adapt<Models.Models.ConsultationRequest>();
            consultationRequest.CreatedDate = DateTime.Today;
            await _unitOfWorkRepository.ConsultationRequestRepository.AddAsync(consultationRequest);
            await _unitOfWorkRepository.SaveChangesAsync();
            return consultationRequest.Adapt<GetConsultationRequestDto>();
        }

        public async Task<GetConsultationRequestDto> UpdateConsultationRequestAsync(GetConsultationRequestDto consultationRequestDto)
        {
            var consultationRequest = await _unitOfWorkRepository.ConsultationRequestRepository.FirstOrDefaultAsync(c => c.Id == consultationRequestDto.Id);
            if (consultationRequest == null)
            {
                return null;
            }
            consultationRequestDto.Adapt(consultationRequest);
            _unitOfWorkRepository.ConsultationRequestRepository.Update(consultationRequest);
            await _unitOfWorkRepository.SaveChangesAsync();
            return consultationRequest.Adapt<GetConsultationRequestDto>();
        }

        public async Task<bool> DeleteConsultationRequestAsync(int id)
        {
            var consultationRequest = await _unitOfWorkRepository.ConsultationRequestRepository.FirstOrDefaultAsync(c => c.Id == id);
            if (consultationRequest == null)
            {
                return false;
            }
            _unitOfWorkRepository.ConsultationRequestRepository.Remove(consultationRequest);
            await _unitOfWorkRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HandleConsultationRequestAsync(ClaimsPrincipal claimsPrincipal, HandleConsultationRequestDto requestDtos)
        {
            var operatorId = int.Parse(claimsPrincipal.FindFirst(c => c.Type == "id")?.Value);

            var recordInDb = await _unitOfWorkRepository.ConsultationRequestRepository
                .FirstOrDefaultAsync(l => l.Id == requestDtos.Id && l.Status != ConsultationRequestStatus.Completed);

            if (recordInDb == null) return false;

            recordInDb.HandleById = operatorId;
            if (requestDtos.Status == ConsultationRequestStatus.Completed)
            {
                recordInDb.Status = ConsultationRequestStatus.Completed;
                recordInDb.HandleById = operatorId;
                _unitOfWorkRepository.ConsultationRequestRepository.Update(recordInDb);
            }
            else if (requestDtos.Status == ConsultationRequestStatus.Failed)
            {
                recordInDb.Status = ConsultationRequestStatus.Failed;
                recordInDb.HandleById = operatorId;
                _unitOfWorkRepository.ConsultationRequestRepository.Update(recordInDb);
            }


            return true;

        }

        public async Task<List<GetConsultationRequestDto>> ViewAllConsultationsRequestAsync(ConsultationRequestFilterDto request)
        {
            return await _unitOfWorkRepository.ConsultationRequestRepository.ViewAllConsultationsRequestAsync(request);
        }
    }
}


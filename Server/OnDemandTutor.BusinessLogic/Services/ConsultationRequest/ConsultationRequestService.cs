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
        private readonly IUnitOfWorkRepository _unitOfWork;

        public ConsultationRequestService(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<GetConsultationRequestDto>> GetConsultationRequestsAsync(PagingModel<GetConsultationRequestDto> pagingModel)
        {
            var pagedResult = await _unitOfWork.ConsultationRequestRepository.PagingAsync(pagingModel.Adapt<PagingModel<Models.Models.ConsultationRequest>>());
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
            var consultationRequest = await _unitOfWork.ConsultationRequestRepository.FirstOrDefaultAsync(c => c.Id == id);
            return consultationRequest?.Adapt<GetConsultationRequestDto>();
        }

        public async Task<GetConsultationRequestDto> CreateConsultationRequestAsync(GetConsultationRequestDto consultationRequestDto)
        {
            var consultationRequest = consultationRequestDto.Adapt<Models.Models.ConsultationRequest>();
            await _unitOfWork.ConsultationRequestRepository.AddAsync(consultationRequest);
            await _unitOfWork.SaveChangesAsync();
            return consultationRequest.Adapt<GetConsultationRequestDto>();
        }

        public async Task<GetConsultationRequestDto> UpdateConsultationRequestAsync(GetConsultationRequestDto consultationRequestDto)
        {
            var consultationRequest = await _unitOfWork.ConsultationRequestRepository.FirstOrDefaultAsync(c => c.Id == consultationRequestDto.Id);
            if (consultationRequest == null)
            {
                return null;
            }
            consultationRequestDto.Adapt(consultationRequest);
            _unitOfWork.ConsultationRequestRepository.Update(consultationRequest);
            await _unitOfWork.SaveChangesAsync();
            return consultationRequest.Adapt<GetConsultationRequestDto>();
        }

        public async Task<bool> DeleteConsultationRequestAsync(int id)
        {
            var consultationRequest = await _unitOfWork.ConsultationRequestRepository.FirstOrDefaultAsync(c => c.Id == id);
            if (consultationRequest == null)
            {
                return false;
            }
            _unitOfWork.ConsultationRequestRepository.Remove(consultationRequest);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HandleConsultationRequestAsync(ClaimsPrincipal claimsPrincipal, HandleConsultationRequestDto requestDtos)
        {
            var operatorId = int.Parse(claimsPrincipal.FindFirst(c => c.Type == "id")?.Value);

            var recordInDb = await _unitOfWork.ConsultationRequestRepository
                .FirstOrDefaultAsync(l => l.Id == requestDtos.Id && l.Status != ConsultationRequestStatus.Completed);

            if (recordInDb == null) return false;

            recordInDb.HandleById = operatorId;
            if (requestDtos.Status == ConsultationRequestStatus.Completed)
            {
                recordInDb.Status = ConsultationRequestStatus.Completed;
                recordInDb.HandleById = operatorId;
                _unitOfWork.ConsultationRequestRepository.Update(recordInDb);
            }
            else if (requestDtos.Status == ConsultationRequestStatus.Failed)
            {
                recordInDb.Status = ConsultationRequestStatus.Failed;
                recordInDb.HandleById = operatorId;
                recordInDb.ReasonFailed = requestDtos.ReasonFailed;
                _unitOfWork.ConsultationRequestRepository.Update(recordInDb);
            }


            return true;

        }
    }
}


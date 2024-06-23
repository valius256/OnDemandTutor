using System;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.DataAccess;
using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Paging;
using Mapster;

namespace OnDemandTutor.BusinessLogic.Services.ConsultationRequest
{
    public class ConsultationRequestService : IConsultationRequestService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public ConsultationRequestService(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<GetConsultationRequestDtos>> GetConsultationRequestsAsync(PagingModel<GetConsultationRequestDtos> pagingModel)
        {
            var pagedResult = await _unitOfWork.ConsultationRequestRepository.PagingAsync(pagingModel.Adapt<PagingModel<Models.Models.ConsultationRequest>>());
            var dtoPagedResult = new PagedResult<GetConsultationRequestDtos>
            {
                Items = pagedResult.Items.Adapt<List<GetConsultationRequestDtos>>(),
                Page = pagedResult.Page,
                Limit = pagedResult.Limit,
                Total = pagedResult.Total
            };
            return dtoPagedResult;
        }

        public async Task<GetConsultationRequestDtos> GetConsultationRequestByIdAsync(int id)
        {
            var consultationRequest = await _unitOfWork.ConsultationRequestRepository.FirstOrDefaultAsync(c => c.Id == id);
            return consultationRequest?.Adapt<GetConsultationRequestDtos>();
        }

        public async Task<GetConsultationRequestDtos> CreateConsultationRequestAsync(GetConsultationRequestDtos consultationRequestDto)
        {
            var consultationRequest = consultationRequestDto.Adapt<Models.Models.ConsultationRequest>();
            await _unitOfWork.ConsultationRequestRepository.AddAsync(consultationRequest);
            await _unitOfWork.SaveChangesAsync();
            return consultationRequest.Adapt<GetConsultationRequestDtos>();
        }

        public async Task<GetConsultationRequestDtos> UpdateConsultationRequestAsync(GetConsultationRequestDtos consultationRequestDto)
        {
            var consultationRequest = await _unitOfWork.ConsultationRequestRepository.FirstOrDefaultAsync(c => c.Id == consultationRequestDto.Id);
            if (consultationRequest == null)
            {
                return null;
            }
            consultationRequestDto.Adapt(consultationRequest);
            _unitOfWork.ConsultationRequestRepository.Update(consultationRequest);
            await _unitOfWork.SaveChangesAsync();
            return consultationRequest.Adapt<GetConsultationRequestDtos>();
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
    }
}


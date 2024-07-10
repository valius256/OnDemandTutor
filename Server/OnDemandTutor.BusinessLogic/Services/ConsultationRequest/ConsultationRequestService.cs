using Mapster;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Paging;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Services.ConsultationRequest
{
    public class ConsultationRequestService : IConsultationRequestService
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IAuthServices _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ConsultationRequestService(IUnitOfWorkRepository unitOfWorkRepository, IAuthServices authService, IHttpContextAccessor HttpContextAccessor)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _authService = authService;
            _httpContextAccessor = HttpContextAccessor;
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
            // Retrieve the existing consultation request entity from the database
            var consultationRequest = await _unitOfWorkRepository.ConsultationRequestRepository.FirstOrDefaultAsync(c => c.Id == consultationRequestDto.Id);

            // Check if the entity is null
            if (consultationRequest == null)
            {
                throw new NotFoundException($"Consultation request with ID {consultationRequestDto.Id} not found.");
            }

            // Get the current user from the authentication service
            var user = await _authService.GetUserProfileByClaim(_httpContextAccessor.HttpContext.User);

            // Adapt the incoming DTO to update the existing entity
            consultationRequestDto.Adapt(consultationRequest);

            // Set the updated fields
            consultationRequest.UpdatedById = user.Id; // Assuming there is an UpdatedById property
            consultationRequest.UpdatedDate = DateTime.Now; // Assuming there is an UpdatedDate property

            // Update the entity in the repository
            var updatedConsultationRequestEntity = _unitOfWorkRepository.ConsultationRequestRepository.Update(consultationRequest);

            // Save changes to the database
            await _unitOfWorkRepository.SaveChangesAsync();

            // Return the updated DTO
            return updatedConsultationRequestEntity.Adapt<GetConsultationRequestDto>();
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
                .FirstOrDefaultAsync(l => l.Id == requestDtos.Id);

            if (recordInDb == null) return false;

            recordInDb.HandleById = operatorId;
            recordInDb.Status = requestDtos.Status;
            _unitOfWorkRepository.ConsultationRequestRepository.Update(recordInDb);
            await _unitOfWorkRepository.SaveChangesAsync();
            return true;

        }

        public async Task<PagedResult<GetConsultationRequestDto>> ViewAllConsultationsRequestAsync(ConsultationRequestFilterDto request)
        {
            var rs = await _unitOfWorkRepository.ConsultationRequestRepository.ViewAllConsultationsRequestAsync(request);
            return rs.Adapt<PagedResult<GetConsultationRequestDto>>();
        }
    }
}


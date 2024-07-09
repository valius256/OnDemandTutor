using Mapster;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.TutorVideo;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.TutorVideo;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.TutorVideo
{
    public class TutorVideoService : ITutorVideoService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        private readonly IAuthServices _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TutorVideoService(IUnitOfWorkRepository unitOfWork, IAuthServices authService, IHttpContextAccessor HttpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _httpContextAccessor = HttpContextAccessor;
        }

        public async Task<PagedResult<GetTutorVideoDto>> GetTutorVideosAsync(PagingModel<GetTutorVideoDto> request)
        {
            var pagedTutorVideos = await _unitOfWork.TutorVideoRepository.PagingAsync(request.Adapt<PagingModel<Models.Models.TutorVideo>>());
            return pagedTutorVideos.Adapt<PagedResult<GetTutorVideoDto>>();
        }

        public async Task<GetTutorVideoDto> GetTutorVideoByIdAsync(int id)
        {
            var tutorVideoEntity = await _unitOfWork.TutorVideoRepository.FirstOrDefaultAsync(tv => tv.Id == id);
            if (tutorVideoEntity == null)
            {
                throw new NotFoundException($"TutorVideo with ID {id} not found.");
            }
            return tutorVideoEntity.Adapt<GetTutorVideoDto>();
        }

        public async Task<CreateTutorVideoDto> CreateTutorVideoAsync(CreateTutorVideoDto tutorVideoDto)
        {
            var tutorVideoEntity = tutorVideoDto.Adapt<Models.Models.TutorVideo>();
            var createdTutorVideoEntity = await _unitOfWork.TutorVideoRepository.AddAsync(tutorVideoEntity);
            await _unitOfWork.SaveChangesAsync();
            return createdTutorVideoEntity.Adapt<CreateTutorVideoDto>();
        }

        public async Task<UpdateTutorVideoDto> UpdateTutorVideoAsync(UpdateTutorVideoDto tutorVideoDto)
        {
            // Retrieve the existing tutor video entity from the database
            var existingTutorVideoEntity = await _unitOfWork.TutorVideoRepository.FirstOrDefaultAsync(tv => tv.Id == tutorVideoDto.Id);

            // Check if the entity is null
            if (existingTutorVideoEntity == null)
            {
                throw new NotFoundException($"TutorVideo with ID {tutorVideoDto.Id} not found.");
            }

            // Get the current user from the authentication service
            var user = await _authService.GetUserProfileByClaim(_httpContextAccessor.HttpContext.User);

            // Adapt the incoming DTO to the existing entity
            existingTutorVideoEntity = tutorVideoDto.Adapt(existingTutorVideoEntity);

            // Set the updated fields
            existingTutorVideoEntity.UpdatedById = user.Id; // Assuming there is an UpdatedById property
            existingTutorVideoEntity.UpdatedDate = DateTime.Now; // Assuming there is an UpdatedDate property

            // Update the entity in the repository
            var updatedTutorVideoEntity = _unitOfWork.TutorVideoRepository.Update(existingTutorVideoEntity);

            // Save changes to the database
            await _unitOfWork.SaveChangesAsync();

            // Return the updated DTO
            return updatedTutorVideoEntity.Adapt<UpdateTutorVideoDto>();
        }


        public async Task<bool> DeleteTutorVideoAsync(int id)
        {
            var existingTutorVideoEntity = await _unitOfWork.TutorVideoRepository.FirstOrDefaultAsync(tv => tv.Id == id);
            if (existingTutorVideoEntity == null)
            {
                throw new NotFoundException($"TutorVideo with ID {id} not found.");
            }

            _unitOfWork.TutorVideoRepository.Remove(existingTutorVideoEntity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
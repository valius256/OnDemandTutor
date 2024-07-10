using Mapster;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.TutorDegree;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.TutorDegree;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.TutorDegreeService
{
    public class TutorDegreeService : ITutorDegreeService
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IAuthServices _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public TutorDegreeService(IUnitOfWorkRepository unitOfWorkRepository, IAuthServices authService, IHttpContextAccessor HttpContextAccessor)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _authService = authService;
            _httpContextAccessor = HttpContextAccessor;
        }

        public async Task<PagedResult<GetTutorDegreeDto>> GetTutorDegreesAsync(PagingModel<GetTutorDegreeDto> request)
        {
            var pagedTutorDegrees = await _unitOfWorkRepository.TutorDegreeRepository.PagingAsync(request.Adapt<PagingModel<Models.Models.TutorDegree>>());
            return pagedTutorDegrees.Adapt<PagedResult<GetTutorDegreeDto>>();
        }

        public async Task<GetTutorDegreeDto> GetTutorDegreeByIdAsync(int id)
        {
            var tutorDegree = await _unitOfWorkRepository.TutorDegreeRepository.FirstOrDefaultAsync(td => td.Id == id);
            if (tutorDegree == null)
            {
                throw new NotFoundException($"TutorDegree with ID {id} not found.");
            }
            return tutorDegree.Adapt<GetTutorDegreeDto>();
        }

        public async Task<CreateTutorDegreeDto> CreateTutorDegreeAsync(CreateTutorDegreeDto tutorDegreeDto)
        {
            var tutorDegree = tutorDegreeDto.Adapt<Models.Models.TutorDegree>();
            var createdTutorDegree = await _unitOfWorkRepository.TutorDegreeRepository.AddAsync(tutorDegree);
            await _unitOfWorkRepository.SaveChangesAsync();
            return createdTutorDegree.Entity.Adapt<CreateTutorDegreeDto>();
        }

        public async Task<UpdateTutorDegreeDto> UpdateTutorDegreeAsync(UpdateTutorDegreeDto tutorDegreeDto)
        {
            // Retrieve the existing tutor degree entity from the database
            var existingTutorDegree = await _unitOfWorkRepository.TutorDegreeRepository.FirstOrDefaultAsync(td => td.Id == tutorDegreeDto.Id);

            // Check if the entity is null
            if (existingTutorDegree == null)
            {
                throw new NotFoundException($"TutorDegree with ID {tutorDegreeDto.Id} not found.");
            }

            // Get the current user from the authentication service
            var user = await _authService.GetUserProfileByClaim(_httpContextAccessor.HttpContext.User);

            // Adapt the incoming DTO to the existing entity
            existingTutorDegree = tutorDegreeDto.Adapt(existingTutorDegree);

            // Set the updated fields
            existingTutorDegree.UpdatedById = user.Id; // Assuming there is an UpdatedById property
            existingTutorDegree.UpdatedDate = DateTime.Now; // Assuming there is an UpdatedDate property

            // Update the entity in the database
            var updatedTutorDegree = _unitOfWorkRepository.TutorDegreeRepository.Update(existingTutorDegree);

            // Save the changes
            await _unitOfWorkRepository.SaveChangesAsync();

            // Return the updated DTO
            return updatedTutorDegree.Entity.Adapt<UpdateTutorDegreeDto>();
        }

        public async Task<bool> DeleteTutorDegreeAsync(int id)
        {
            var existingTutorDegree = await _unitOfWorkRepository.TutorDegreeRepository.FirstOrDefaultAsync(td => td.Id == id);
            if (existingTutorDegree == null)
            {
                throw new NotFoundException($"TutorDegree with ID {id} not found.");
            }

            _unitOfWorkRepository.TutorDegreeRepository.Remove(existingTutorDegree);
            await _unitOfWorkRepository.SaveChangesAsync();

            return true;
        }
    }
}
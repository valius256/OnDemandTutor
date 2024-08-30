using Mapster;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.TutorVideo;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.TutorVideo;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.TutorVideo
{
    public class TutorVideoService : ITutorVideoService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;


        public TutorVideoService(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<GetTutorVideoDto>> GetTutorVideosAsync(PagingModel<QueryTutorVideoDto> request)
        {
            var pagedTutorVideos = await _unitOfWork.TutorVideoRepository.QueryTutorVideoAsync(request);
            return pagedTutorVideos.Adapt<PagedResult<GetTutorVideoDto>>();
        }

        public async Task<GetTutorVideoDto> GetTutorVideoByIdAsync(int id)
        {
            var tutorVideoEntity = await _unitOfWork.TutorVideoRepository.GetTutorVideoByIdAsync(id);
            if (tutorVideoEntity == null)
            {
                throw new DataNotFoundException($"TutorVideo with ID {id} not found.");
            }
            return tutorVideoEntity.Adapt<GetTutorVideoDto>();
        }

        public async Task<GetTutorVideoDto> CreateTutorVideoAsync(CreateTutorVideoDto tutorVideoDto, GetProfileUserDto user)
        {
            var tutorVideoEntity = tutorVideoDto.Adapt<Models.Models.TutorVideo>();
            tutorVideoEntity.TutorId = user.Id;

            var createdTutorVideoEntity = await _unitOfWork.TutorVideoRepository.AddAsync(tutorVideoEntity);
            await _unitOfWork.SaveChangesAsync();

            return createdTutorVideoEntity.Entity.Adapt<GetTutorVideoDto>();
        }

        public async Task<GetTutorVideoDto> UpdateTutorVideoAsync(UpdateTutorVideoDto tutorVideoDto, GetProfileUserDto user)
        {
            // Retrieve the existing tutor video entity from the database
            var existingTutorVideoEntity = await _unitOfWork.TutorVideoRepository.FirstOrDefaultAsync(tv => tv.Id == tutorVideoDto.Id);

            // Check if the entity is null
            if (existingTutorVideoEntity == null)
            {
                throw new DataNotFoundException($"TutorVideo with ID {tutorVideoDto.Id} not found.");
            }


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
            return updatedTutorVideoEntity.Adapt<GetTutorVideoDto>();
        }


        public async Task<bool> DeleteTutorVideoAsync(int id)
        {
            var existingTutorVideoEntity = await _unitOfWork.TutorVideoRepository.FirstOrDefaultAsync(tv => tv.Id == id);
            if (existingTutorVideoEntity == null)
            {
                throw new DataNotFoundException($"TutorVideo with ID {id} not found.");
            }

            _unitOfWork.TutorVideoRepository.Remove(existingTutorVideoEntity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
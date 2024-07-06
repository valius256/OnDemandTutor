using Mapster;
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

        public TutorVideoService(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            var existingTutorVideoEntity = await _unitOfWork.TutorVideoRepository.FirstOrDefaultAsync(tv => tv.Id == tutorVideoDto.Id);
            if (existingTutorVideoEntity == null)
            {
                throw new NotFoundException($"TutorVideo with ID {tutorVideoDto.Id} not found.");
            }

            existingTutorVideoEntity = tutorVideoDto.Adapt(existingTutorVideoEntity);

            var updatedTutorVideoEntity = _unitOfWork.TutorVideoRepository.Update(existingTutorVideoEntity);
            await _unitOfWork.SaveChangesAsync();

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
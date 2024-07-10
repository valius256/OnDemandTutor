using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.TutorSubject;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.TutorSubject;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.TutorSubject
{
    public class TutorSubjectService : ITutorSubjectService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public TutorSubjectService(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<GetTutorSubjectDto>> GetTutorSubjectsAsync(PagingModel<GetTutorSubjectDto> request)
        {
            var pagedTutorSubjects = await _unitOfWork.TutorSubjectRepository.PagingAsync(request.Adapt<PagingModel<GetTutorSubjectDto>>());
            return pagedTutorSubjects.Adapt<PagedResult<GetTutorSubjectDto>>();
        }

        public async Task<GetTutorSubjectDto> GetTutorSubjectByIdAsync(int id)
        {
            var tutorSubjectEntity = await _unitOfWork.TutorSubjectRepository.FirstOrDefaultAsync(ts => ts.UserId == id);
            if (tutorSubjectEntity == null)
            {
                throw new NotFoundException($"TutorSubject with ID {id} not found.");
            }
            return tutorSubjectEntity.Adapt<GetTutorSubjectDto>();
        }

        public async Task<CreateTutorSubjectDto> CreateTutorSubjectAsync(CreateTutorSubjectDto tutorSubjectDto)
        {
            var tutorSubjectEntity = tutorSubjectDto.Adapt<Models.Models.TutorSubject>();
            var createdTutorSubjectEntity = await _unitOfWork.TutorSubjectRepository.AddAsync(tutorSubjectEntity);
            await _unitOfWork.SaveChangesAsync();
            return createdTutorSubjectEntity.Adapt<CreateTutorSubjectDto>();
        }

        public async Task<UpdateTutorSubjectDto> UpdateTutorSubjectAsync(UpdateTutorSubjectDto tutorSubjectDto)
        {
            var existingTutorSubjectEntity = await _unitOfWork.TutorSubjectRepository.FirstOrDefaultAsync(ts => ts.Id == tutorSubjectDto.Id);
            if (existingTutorSubjectEntity == null)
            {
                throw new NotFoundException($"TutorSubject with ID {tutorSubjectDto.Id} not found.");
            }

            existingTutorSubjectEntity = tutorSubjectDto.Adapt(existingTutorSubjectEntity);

            var updatedTutorSubjectEntity = _unitOfWork.TutorSubjectRepository.Update(existingTutorSubjectEntity);
            await _unitOfWork.SaveChangesAsync();

            return updatedTutorSubjectEntity.Adapt<UpdateTutorSubjectDto>();
        }

        public async Task<bool> DeleteTutorSubjectAsync(int id)
        {
            var existingTutorSubjectEntity = await _unitOfWork.TutorSubjectRepository.FirstOrDefaultAsync(ts => ts.Id == id);
            if (existingTutorSubjectEntity == null)
            {
                throw new NotFoundException($"TutorSubject with ID {id} not found.");
            }

            _unitOfWork.TutorSubjectRepository.Remove(existingTutorSubjectEntity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
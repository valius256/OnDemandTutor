using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.TutorDegree;
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
        private readonly ITutorDegreeService _tutorDegreeService;

        public TutorSubjectService(IUnitOfWorkRepository unitOfWork, ITutorDegreeService tutorDegreeService)
        {
            _unitOfWork = unitOfWork;
            _tutorDegreeService = tutorDegreeService;
        }

        public async Task<PagedResult<GetTutorSubjectDto>> GetTutorSubjectsAsync(PagingModel<QueryTutorSubjectDto> request)
        {
            var pagedTutorSubjects = await _unitOfWork.TutorSubjectRepository.GetTutorSubjects(request);
            return pagedTutorSubjects.Adapt<PagedResult<GetTutorSubjectDto>>();
        }

        public async Task<GetTutorSubjectDetailDto> GetTutorSubjectByIdAsync(int id)
        {
            var tutorSubjectEntity = await _unitOfWork.TutorSubjectRepository.GetTutorSubjectById(id);
            if (tutorSubjectEntity == null)
            {
                throw new NotFoundException($"TutorSubject with ID {id} not found.");
            }
            var mappedTutorSubject = tutorSubjectEntity.Adapt<GetTutorSubjectDetailDto>();
            mappedTutorSubject.Degrees = await _tutorDegreeService.GetTutorDegreesByTutorIdAndSubjectId(mappedTutorSubject.UserId, mappedTutorSubject.SubjectId);
            return mappedTutorSubject;
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
using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.TutorDegree;
using OnDemandTutor.BusinessLogic.Interfaces.TutorSubject;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.TutorDegree;
using OnDemandTutor.Models.Dtos.TutorSubject;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.TutorSubject;

public class TutorSubjectService : ITutorSubjectService
{
    private readonly ITutorDegreeService _tutorDegreeService;
    private readonly IUnitOfWorkRepository _unitOfWork;

    public TutorSubjectService(IUnitOfWorkRepository unitOfWork, ITutorDegreeService tutorDegreeService)
    {
        _unitOfWork = unitOfWork;
        _tutorDegreeService = tutorDegreeService;
    }

    public async Task<PagedResult<GetTutorSubjectWithUserAndSubjectDto>> GetTutorSubjectsAsync(
        PagingModel<QueryTutorSubjectDto> request)
    {
        var pagedTutorSubjects = await _unitOfWork.TutorSubjectRepository.GetTutorSubjects(request);
        return pagedTutorSubjects.Adapt<PagedResult<GetTutorSubjectWithUserAndSubjectDto>>();
    }

    public async Task<GetTutorSubjectDetailDto> GetTutorSubjectByIdAsync(int id)
    {
        var tutorSubjectEntity = await _unitOfWork.TutorSubjectRepository.GetTutorSubjectById(id);
        if (tutorSubjectEntity == null) throw new NotFoundException($"TutorSubject with ID {id} not found.");
        var mappedTutorSubject = tutorSubjectEntity.Adapt<GetTutorSubjectDetailDto>();
        mappedTutorSubject.Degrees =
            await _tutorDegreeService.GetTutorDegreesByTutorIdAndSubjectId(mappedTutorSubject.UserId,
                mappedTutorSubject.SubjectId);
        return mappedTutorSubject;
    }

    public async Task<GetTutorSubjectDetailDto> CreateTutorSubjectAsync(CreateTutorSubjectDto tutorSubjectDto)
    {
        var tutorSubjectEntity = tutorSubjectDto.Adapt<Models.Models.TutorSubject>();
        var createdTutorSubjectEntity = await _unitOfWork.TutorSubjectRepository.AddAsync(tutorSubjectEntity);
        foreach (var degree in tutorSubjectDto.Degrees)
        {
            var createDto = degree.Adapt<CreateTutorDegreeDto>();
            createDto.TutorId = tutorSubjectDto.UserId;
            createDto.SubjectId = tutorSubjectDto.SubjectId;
            await _tutorDegreeService.CreateTutorDegreeAsync(createDto);
        }

        createdTutorSubjectEntity.Entity.Status = TutorSubjectStatus.Pending;
        await _unitOfWork.SaveChangesAsync();
        return createdTutorSubjectEntity.Entity.Adapt<GetTutorSubjectDetailDto>();
    }

    public async Task<UpdateTutorSubjectDto> UpdateTutorSubjectAsync(UpdateTutorSubjectDto tutorSubjectDto)
    {
        var existingTutorSubjectEntity =
            await _unitOfWork.TutorSubjectRepository.FirstOrDefaultAsync(ts => ts.Id == tutorSubjectDto.Id);
        if (existingTutorSubjectEntity == null)
            throw new NotFoundException($"TutorSubject with ID {tutorSubjectDto.Id} not found.");

        existingTutorSubjectEntity = tutorSubjectDto.Adapt(existingTutorSubjectEntity);

        var updatedTutorSubjectEntity = _unitOfWork.TutorSubjectRepository.Update(existingTutorSubjectEntity);
        await _unitOfWork.SaveChangesAsync();

        return updatedTutorSubjectEntity.Adapt<UpdateTutorSubjectDto>();
    }

    public async Task<bool> DeleteTutorSubjectAsync(int id)
    {
        var existingTutorSubjectEntity =
            await _unitOfWork.TutorSubjectRepository.FirstOrDefaultAsync(ts => ts.Id == id);
        if (existingTutorSubjectEntity == null) throw new NotFoundException($"TutorSubject with ID {id} not found.");

        _unitOfWork.TutorSubjectRepository.Remove(existingTutorSubjectEntity);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
using Mapster;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Subject;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IAuthServices _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubjectService(IUnitOfWorkRepository unitOfWork, IHttpContextAccessor httpContextAccessor, IAuthServices authService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }

        //public async Task<PagedResult<GetSubjectDtos>> GetSubjectsAsync(PagingModel<GetSubjectDtos> request)
        //{
        //    var pagedSubjects = await _unitOfWork.SubjectRepository.PagingAsync(request.Adapt<PagingModel<Models.Models.Subject>>());
        //    return pagedSubjects.Adapt<PagedResult<GetSubjectDtos>>();
        //}
        public async Task<PagedResult<GetSubjectDtos>> GetSubjectsAsync(PagingModel<QuerySubjectDTO> request)
        {
            var pagedSubjects = await _unitOfWork.SubjectRepository.GetSubjects(request);
            return pagedSubjects.Adapt<PagedResult<GetSubjectDtos>>();
        }

        public async Task<GetSubjectDtos> GetSubjectByIdAsync(int id)
        {
            var subject = await _unitOfWork.SubjectRepository.FirstOrDefaultAsync(s => s.Id == id);
            if (subject == null)
            {
                throw new NotFoundException($"Subject with ID {id} not found.");
            }
            var subjectDto = subject.Adapt<GetSubjectDtos>();

            return subjectDto;
        }
        public async Task<GetSubjectDtos> CreateSubjectAsync(CreateSubjectDtos subjectCreateDto)
        {
            var subjectEntity = subjectCreateDto.Adapt<Models.Models.Subject>();
            if (await _unitOfWork.SubjectRepository.AnyAsync(sb => sb.Name == subjectCreateDto.Name))
            {
                throw new ModelException($"{subjectEntity}", "has dupplicated", "dupplicated");
            }
            var createdSubjectEntity = await _unitOfWork.SubjectRepository.AddAsync(subjectEntity);
            await _unitOfWork.SaveChangesAsync();
            return createdSubjectEntity.Adapt<GetSubjectDtos>();
        }


        public async Task<GetSubjectDtos> UpdateSubjectAsync(UpdateSubjectDtos subjectGetDto)
        {
            var existingSubject = await _unitOfWork.SubjectRepository.FirstOrDefaultAsync(s => s.Id == subjectGetDto.Id);
            if (existingSubject == null)
            {
                throw new NotFoundException($"Subject with ID {subjectGetDto.Id} not found.");
            }

            var user = await _authService.GetUserProfileByClaim(_httpContextAccessor.HttpContext.User);
            existingSubject = subjectGetDto.Adapt(existingSubject);
            existingSubject.UpdatedDate = DateTime.Now; // Update this field if needed

            var updatedSubject = _unitOfWork.SubjectRepository.Update(existingSubject);
            await _unitOfWork.SaveChangesAsync();

            return updatedSubject.Entity.Adapt<GetSubjectDtos>();
        }

        public async Task<bool> DeleteSubjectAsync(int id)
        {
            var existingSubject = await _unitOfWork.SubjectRepository.FirstOrDefaultAsync(s => s.Id == id);
            if (existingSubject == null)
            {
                throw new NotFoundException($"Subject with ID {id} not found.");
            }

            _unitOfWork.SubjectRepository.Remove(existingSubject);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }


    }
}

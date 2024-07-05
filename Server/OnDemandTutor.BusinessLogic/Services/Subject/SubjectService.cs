using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.BusinessLogic.Interfaces.Subject;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Paging;
using System.Threading.Tasks;

namespace OnDemandTutor.BusinessLogic.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public SubjectService(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<GetSubjectDtos>> GetSubjectsAsync(PagingModel<GetSubjectDtos> request)
        {
            var pagedSubjects = await _unitOfWork.SubjectRepository.PagingAsync(request.Adapt<PagingModel<Models.Models.Subject>>());
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
        public async Task<CreateSubjectDtos> CreateSubjectAsync(CreateSubjectDtos subjectCreateDto)
        {
            var subjectEntity = subjectCreateDto.Adapt<Models.Models.Subject>();
            var createdSubjectEntity = await _unitOfWork.SubjectRepository.AddAsync(subjectEntity);
            await _unitOfWork.SaveChangesAsync();
            return createdSubjectEntity.Adapt<CreateSubjectDtos>();
        }
        public async Task<GetSubjectDtos> UpdateSubjectAsync(GetSubjectDtos subjectGetDto)
        {
            var existingSubject = await _unitOfWork.SubjectRepository.FirstOrDefaultAsync(s => s.Id == subjectGetDto.Id);
            if (existingSubject == null)
            {
                throw new NotFoundException($"Subject with ID {subjectGetDto.Id} not found.");
            }

            existingSubject = subjectGetDto.Adapt(existingSubject);

            var updatedSubject = _unitOfWork.SubjectRepository.Update(existingSubject);
            await _unitOfWork.SaveChangesAsync();

            return updatedSubject.Adapt<GetSubjectDtos>();
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

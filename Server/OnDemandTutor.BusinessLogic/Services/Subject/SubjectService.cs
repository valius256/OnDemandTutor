
using System;
using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.Subject;
using OnDemandTutor.DataAccess;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.Subject
{
	public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public SubjectService(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<GetSubjectDtos>> GetAllSubjects()
        {
            var subjects = await _unitOfWork.SubjectRepository.ToListAsync();
            return subjects.Adapt<List<GetSubjectDtos>>();
        }

        public async Task<GetSubjectDtos> GetSubjectById(int id)
        {
            var subject = await _unitOfWork.SubjectRepository.FirstOrDefaultAsync(s => s.Id == id);
            return subject?.Adapt<GetSubjectDtos>();
        }

        public async Task<CreateSubjectDtos> CreateSubject(CreateSubjectDtos subjectDto)
        {
            var subjectEntity = subjectDto.Adapt<Models.Models.Subject>();
            var createdSubjectEntity = await _unitOfWork.SubjectRepository.AddAsync(subjectEntity);
            await _unitOfWork.SaveChangesAsync();
            return createdSubjectEntity.Adapt<CreateSubjectDtos>();
        }

        public async Task<UpdateSubjectDtos> UpdateSubject(UpdateSubjectDtos subjectDto)
        {
            var existingSubject = await _unitOfWork.SubjectRepository.FindAsync(subjectDto.Id);
            if (existingSubject == null)
            {
                throw new ArgumentException("Subject not found");
            }

            existingSubject = subjectDto.Adapt(existingSubject);
            var updatedSubjectEntity = _unitOfWork.SubjectRepository.Update(existingSubject);
            await _unitOfWork.SaveChangesAsync();
            return updatedSubjectEntity.Adapt<UpdateSubjectDtos>();
        }

        public async Task<bool> DeleteSubject(int id)
        {
            var existingSubject = await _unitOfWork.SubjectRepository.FindAsync(id);
            if (existingSubject == null)
            {
                throw new ArgumentException("Subject not found");
            }

            _unitOfWork.SubjectRepository.Remove(existingSubject);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<GetSubjectDtos>> SearchSubjectsByName(string name)
        {
            var subjects = await _unitOfWork.SubjectRepository.WhereAsync(s => s.Name.Contains(name));
            return subjects.Adapt<List<GetSubjectDtos>>();
        }
        //public async Task<PagedResult<GetSubjectDtos>> GetSubjectsAsync(PagingModel<GetSubjectDtos> pagingModel)
        //{
        //    // Fetch the paginated data from the repository
        //    var pagedResult = await _unitOfWork.SubjectRepository.PagingAsync(pagingModel);

        //    // Use Mapster to map the entities to DTOs
        //    var dtoPagedResult = new PagedResult<GetSubjectDtos>
        //    {
        //        Items = pagedResult.Items.Adapt<List<GetSubjectDtos>>(),
        //        TotalCount = pagedResult.TotalCount,
        //        CurrentPage = pagedResult.CurrentPage,
        //        PageSize = pagedResult.PageSize,
        //        TotalPages = pagedResult.TotalPages
        //    };

        //    return dtoPagedResult;
        //}
        public async Task<PagedResult<GetSubjectDtos>> GetSubjects(PagingModel<GetSubjectDtos> pagingModel)
        {
            var subjects = await _unitOfWork.SubjectRepository.PagingAsync(
                pagingModel,
                x => true, // Add your predicate here if needed
                entities => entities.Adapt<List<GetSubjectDtos>>());

            return subjects;
        }


    }
}


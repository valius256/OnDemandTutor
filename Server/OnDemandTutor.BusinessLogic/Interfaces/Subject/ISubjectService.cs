using System;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Subject
{
	public interface ISubjectService
	{
        Task<List<GetSubjectDtos>> GetAllSubjectsAsync();
        Task<GetSubjectDtos> GetSubjectByIdAsync(int id);
        Task<CreateSubjectDtos> CreateSubjectAsync(CreateSubjectDtos subjectDto);
        Task<UpdateSubjectDtos> UpdateSubjectAsync(UpdateSubjectDtos subjectDto);
        Task<bool> DeleteSubjectAsync(int id);
        Task<List<GetSubjectDtos>> SearchSubjectsByName(string name);
        Task<PagedResult<GetSubjectDtos>> GetSubjectsAsync(PagingModel<GetSubjectDtos> pagingModel);
    }
}


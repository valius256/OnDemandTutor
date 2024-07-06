using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTutor.Models.Dtos.TutorDegree;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.TutorDegree
{
    public interface ITutorDegreeService
    {
        Task<PagedResult<GetTutorDegreeDto>> GetTutorDegreesAsync(PagingModel<GetTutorDegreeDto> request);
        Task<GetTutorDegreeDto> GetTutorDegreeByIdAsync(int id);
        Task<CreateTutorDegreeDto> CreateTutorDegreeAsync(CreateTutorDegreeDto tutorDegreeDto);
        Task<UpdateTutorDegreeDto> UpdateTutorDegreeAsync(UpdateTutorDegreeDto tutorDegreeDto);
        Task<bool> DeleteTutorDegreeAsync(int id);
    }
}
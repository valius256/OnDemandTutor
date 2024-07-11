using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Class
{
    public interface IClassService
    {
        Task<PagedResult<GetClassDtos>> GetClassesAsync(PagingModel<GetClassDtos> pagingModel);
        Task<GetClassDtos> GetClassByIdAsync(int id);
        Task<CreateClassDTO> CreateClassAsync(CreateClassDTO classDto);
        Task<GetClassDtos> UpdateClassAsync(GetClassDtos classDto);
        Task<bool> DeleteClassAsync(int id);
        Task<GetClassFullDataSlotDto> GetClassWithFullDataSlotId(int id);
    }
}


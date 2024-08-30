using OnDemandTutor.Models.Dtos.FAQ;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.FAQ
{
    public interface IFAQService
    {
        Task<PagedResult<GetFAQDto>> GetFAQsAsync(PagingModel<QueryFAQDTO> request);
        Task<GetFAQDto> GetFAQByIdAsync(int id);
        Task<CreateFAQDto> CreateFAQAsync(CreateFAQDto faqDto);
        Task<UpdateFAQDto> UpdateFAQAsync(UpdateFAQDto faqDto);
        Task<bool> DeleteFAQAsync(int id);
    }

}


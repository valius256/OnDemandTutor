using System;
using OnDemandTutor.Models.Dtos.FAQ;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.FAQ
{
	public interface IFAQService
	{
        Task<PagedResult<FAQDto>> GetFAQsAsync(PagingModel<FAQDto> pagingModel);
        Task<FAQDto> GetFAQByIdAsync(int id);
        Task<FAQDto> CreateFAQAsync(FAQDto faqDto);
        Task<FAQDto> UpdateFAQAsync(FAQDto faqDto);
        Task<bool> DeleteFAQAsync(int id);
    }
}


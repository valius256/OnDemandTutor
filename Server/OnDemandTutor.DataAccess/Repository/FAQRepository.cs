using System;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.FAQ;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository
{
    public class FAQRepository : GenericRepository<FAQ>, IFAQRepository
    {
        public FAQRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<FAQ>> GetFAQs(PagingModel<QueryFAQDTO> pagingModel)
        {

            var faqQuery = dbSet
                .Include(f => f.CreateBy)
                .AsQueryable();

            if (pagingModel.Filter != null)
            {
                if (pagingModel.Filter.Keyword != null)
                {
                    faqQuery = faqQuery.Where(f => f.Question.Contains(pagingModel.Filter.Keyword)  || (f.Answer != null && f.Answer.Contains(pagingModel.Filter.Keyword)));
                }
                if (!string.IsNullOrWhiteSpace(pagingModel.Filter.Question))
                {
                    faqQuery = faqQuery.Where(f => f.Question.Contains(pagingModel.Filter.Question));
                }

                if (!string.IsNullOrWhiteSpace(pagingModel.Filter.Answer))
                {
                    faqQuery = faqQuery.Where(f => f.Answer.Contains(pagingModel.Filter.Answer));
                }

                if (pagingModel.Filter.CreateFrom.HasValue)
                {
                    faqQuery = faqQuery.Where(f => f.CreatedDate >= pagingModel.Filter.CreateFrom.Value);
                }

                if (pagingModel.Filter.CreateTo.HasValue)
                {
                    faqQuery = faqQuery.Where(f => f.CreatedDate <= pagingModel.Filter.CreateTo.Value);
                }

                if (pagingModel.Filter.UpdateFrom.HasValue)
                {
                    faqQuery = faqQuery.Where(f => f.UpdatedDate >= pagingModel.Filter.UpdateFrom.Value);
                }

                if (pagingModel.Filter.UpdateTo.HasValue)
                {
                    faqQuery = faqQuery.Where(f => f.UpdatedDate <= pagingModel.Filter.UpdateTo.Value);
                }

                if (pagingModel.Filter.CreateBy.HasValue)
                {
                    faqQuery = faqQuery.Where(f => f.CreateById == pagingModel.Filter.CreateBy.Value);
                }
            }

            if (pagingModel.Sorts != null)
            {
                faqQuery = faqQuery.OrderProperty(pagingModel.Sorts);
            }

            int limit = pagingModel.Limit > 0 ? pagingModel.Limit : 10;
            int page = pagingModel.Page > 0 ? pagingModel.Page : 1;
            int skip = (page - 1) * limit;

           

            var pagedResult = await faqQuery.ToNewPagingAsync(page, limit);

            return pagedResult;
        }
    }
}


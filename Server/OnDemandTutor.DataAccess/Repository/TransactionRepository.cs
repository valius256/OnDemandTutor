using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<Transaction>> ViewALlTransaction(TransactionFilterDto transactionFilter, int userId)
    {
        var transactionQuery = dbSet
            .Where(ld => ld.CreatedById == userId || userId == 0)
            .AsQueryable();

        // Apply filters based on transactionFilterDto
        if (transactionFilter.FromDate != default && transactionFilter.ToDate != default)
        {
            transactionQuery = transactionQuery.Where(t => t.CreatedDate >= transactionFilter.FromDate && t.CreatedDate <= transactionFilter.ToDate);
        }

        if (transactionFilter.MinAmount > 0)
        {
            transactionQuery = transactionQuery.Where(t => t.Amount >= transactionFilter.MinAmount);
        }

        if (transactionFilter.MaxAmount > 0)
        {
            transactionQuery = transactionQuery.Where(t => t.Amount <= transactionFilter.MaxAmount);
        }

        if (transactionFilter.TransactionType != null &&  transactionFilter.TransactionType.Any())
        {
            transactionQuery = transactionQuery.Where(t => transactionFilter.TransactionType.Contains(t.TransactionType));
        }


        int limit = transactionFilter.Limit.Value > 0 ? transactionFilter.Limit.Value : 10;
        int page = transactionFilter.Page.Value > 0 ? transactionFilter.Page.Value : 1;
        int skip = (page - 1) * limit;

        var filteredUsers = await transactionQuery
            .AsNoTracking()
            .ToNewPagingAsync(page, limit);

        return filteredUsers;
    }

}
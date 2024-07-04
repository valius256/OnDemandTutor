using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Transaction>> ViewALlTransaction(TransactionFilterDto transactionFilter, int userId)
    {
        var transactionQuery = dbSet
            .Where(ld => ld.CreatedById == userId)
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



        int limit = transactionFilter.Limit > 0 ? transactionFilter.Limit : 10;
        int page = transactionFilter.Page > 0 ? transactionFilter.Page : 1;
        int skip = (page - 1) * limit;
        transactionQuery = transactionQuery.Skip(skip).Take(limit);

        var filteredUsers = await transactionQuery
            .AsNoTracking()
            .ToListAsync();
        return filteredUsers;
    }

}
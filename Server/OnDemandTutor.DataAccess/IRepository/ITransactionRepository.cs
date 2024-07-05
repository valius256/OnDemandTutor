using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Paging;
using Transaction = OnDemandTutor.Models.Models.Transaction;

namespace OnDemandTutor.DataAccess.IRepository;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task<PagedResult<Transaction>> ViewALlTransaction(TransactionFilterDto transaction, int userId);
}
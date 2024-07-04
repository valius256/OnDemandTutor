using OnDemandTutor.Models.Dtos.Transaction;
using Transaction = OnDemandTutor.Models.Models.Transaction;

namespace OnDemandTutor.DataAccess.IRepository;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task<List<Transaction>> ViewALlTransaction(TransactionFilterDto transaction, int userId);
}
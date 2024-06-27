using System.Transactions;
using OnDemandTutor.Models.Dtos.Transaction;
using Transaction = OnDemandTutor.Models.Models.Transaction;

namespace OnDemandTutor.DataAccess.IRepository;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
 
}
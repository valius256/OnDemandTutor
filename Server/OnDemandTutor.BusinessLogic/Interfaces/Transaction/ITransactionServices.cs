using OnDemandTutor.Models.Dtos.Transaction;

namespace OnDemandTutor.BusinessLogic.Interfaces.Transaction;

public interface ITransactionServices
{
    Task<int> CreateTransactionDb(TransactionDto transaction);
    Task<int> TransactionPaid(string transactionId, DateTime paidTime);
}
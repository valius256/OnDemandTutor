using OnDemandTutor.Models.Dtos.Transaction;

namespace OnDemandTutor.BusinessLogic.Interfaces.Transaction;

public interface ITransactionServices
{
    Task CreateTransactionDb(TransactionDtos transaction);
}
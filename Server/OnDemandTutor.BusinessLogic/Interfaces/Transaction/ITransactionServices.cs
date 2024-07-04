using OnDemandTutor.Models.Dtos.Transaction;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Interfaces.Transaction;

public interface ITransactionServices
{
    Task<int> CreateTransactionDb(TransactionDto transaction);
    Task<int> TransactionPaid(string transactionId, DateTime paidTime);
    Task<List<TransactionDto>> ViewALlTransaction(TransactionFilterDto transaction, ClaimsPrincipal userClaim);
}
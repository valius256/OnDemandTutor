using OnDemandTutor.Models.Dtos.Transaction;
using System.Security.Claims;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Transaction;

public interface ITransactionServices
{
    Task<int> CreateTransactionDb(TransactionDto transaction);
    Task<int> TransactionPaid(string transactionId, DateTime paidTime);
    Task<TransactionDto?> GetTransactionById(int id, ClaimsPrincipal? userClaims);
    Task<PagedResult<TransactionDto>> ViewALlTransaction(TransactionFilterDto transaction, ClaimsPrincipal userClaim);
}
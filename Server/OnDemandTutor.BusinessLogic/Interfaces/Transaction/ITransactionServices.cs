using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Paging;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Interfaces.Transaction;

public interface ITransactionServices
{
    Task<int> CreateTransactionDb(List<TransactionDto> transaction);
    Task<int> TransactionPaid(string transactionId, DateTime paidTime);
    Task<TransactionDto?> GetTransactionById(int id, ClaimsPrincipal? userClaims);
    Task<PagedResult<TransactionDto>> ViewALlTransaction(TransactionFilterDto transaction, ClaimsPrincipal userClaim);
}
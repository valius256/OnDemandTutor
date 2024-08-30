using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Interfaces.Transaction;

public interface ITransactionServices
{
    Task<int> CreateTransactionDb(List<GetTransactionDto> transaction);
    Task<int> TransactionPaid(string transactionId, DateTime paidTime);
    Task<GetTransactionDto?> GetTransactionById(int id, GetProfileUserDto user);
    Task<PagedResult<GetTransactionDto>> ViewALlTransaction(TransactionFilterDto transaction, GetProfileUserDto user);

    Task<PagedResult<GetTransactionDto>> ViewALlTransactionAsAdmmin(TransactionFilterDto transaction);
    //Task<bool> CreateTransactionForAutoDecreaMoneySlotAsync(int slotId, decimal amount);
    //Task<bool> CreateTransactionForAutoDecreaMoneySlotFailedAsync(int slotId, decimal amount);
    //Task<int> CreateTransactionForClassPayment(string orderId, int userId, int classId, decimal amount);
}
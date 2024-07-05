using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Enum;
using System.Security.Claims;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.Transaction;

public class TransactionServices : ITransactionServices
{
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;

    public TransactionServices(IUnitOfWorkRepository unitOfWorkRepository)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
    }

    public async Task<int> CreateTransactionDb(TransactionDto transaction)
    {
        var transactionModel = transaction.Adapt<Models.Models.Transaction>();
        await _unitOfWorkRepository.TransactionRepository
            .AddAsync(transactionModel);
        await _unitOfWorkRepository.SaveChangesAsync();
        return transactionModel.Id;
    }

    public async Task<int> TransactionPaid(string transactionId, DateTime paidTime)
    {
        var transactionModel =
            await _unitOfWorkRepository.TransactionRepository.FirstOrDefaultAsync(tr => tr.TransactionCode == transactionId);
        if (transactionModel == null)
        {
            throw new ModelException("Transaction model", "Not Found", "transaction not exist");
        }

        transactionModel.Status = PaymentStatus.Paid;
        transactionModel.UpdatedDate = paidTime;

        _unitOfWorkRepository.TransactionRepository.Update(transactionModel);
        await _unitOfWorkRepository.SaveChangesAsync();
        return transactionModel.Id;
    }

    public async Task<TransactionDto?> GetTransactionById(int id, ClaimsPrincipal? userClaims)
    {
        var uid = userClaims?.FindFirst(cl => cl.Type == "id")?.Value;
        var model = await _unitOfWorkRepository.TransactionRepository.FirstOrDefaultAsync(ts =>
            ts.Id == id && ts.CreatedById == int.Parse(uid));
        return model?.Adapt<TransactionDto>();
    }

    public async Task<PagedResult<TransactionDto>> ViewALlTransaction(TransactionFilterDto transaction, ClaimsPrincipal userClaim)
    {
        var id = userClaim.FindFirst(cl => cl.Type == "id")?.Value;
        var listTransactionModel = await _unitOfWorkRepository.TransactionRepository.ViewALlTransaction(transaction, int.Parse(id));
        return listTransactionModel.Adapt<PagedResult<TransactionDto>>();
    }
}
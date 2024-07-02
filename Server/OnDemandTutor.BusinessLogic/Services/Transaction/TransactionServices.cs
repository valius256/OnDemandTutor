using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Enum;

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
        await _unitOfWorkRepository.TransactionRepository.AddAsync(transactionModel);
        await _unitOfWorkRepository.SaveChangesAsync();
        return transactionModel.Id;
    }

    public async Task TransactionPaid(string transactionId, DateTime paidTime)
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
    }
}
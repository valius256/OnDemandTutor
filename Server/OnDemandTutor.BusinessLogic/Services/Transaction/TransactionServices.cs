using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.DataAccess;
using OnDemandTutor.Models.Dtos.Transaction;

namespace OnDemandTutor.BusinessLogic.Services.Transaction;

public class TransactionServices : ITransactionServices
{
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;

    public TransactionServices(IUnitOfWorkRepository unitOfWorkRepository)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
    }

    public async Task CreateTransactionDb(TransactionDtos transaction)
    {
        // var transactionTbl = await _unitOfWorkRepository.TransactionRepository.AnyAsync();
        //
        //
        //
        // return;''
    }
}
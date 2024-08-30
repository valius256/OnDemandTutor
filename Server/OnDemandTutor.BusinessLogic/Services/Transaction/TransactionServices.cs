using Mapster;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.Transaction;

public class TransactionServices : ITransactionServices
{
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    //public readonly ISlotStudentServices _slotStudentServices;
    private readonly INotificationService _notificationService;

    public TransactionServices(IUnitOfWorkRepository unitOfWorkRepository, INotificationService notificationService)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
        //_slotStudentServices = slotStudentServices;
        _notificationService = notificationService;
    }

    public async Task<int> CreateTransactionDb(List<GetTransactionDto> transaction)
    {
        var transactionModels = transaction.Adapt<List<Models.Models.Transaction>>();
        await _unitOfWorkRepository.TransactionRepository
             .AddRangeAsync(transactionModels);
        var rs = await _unitOfWorkRepository.SaveChangesAsync();
        return rs;
    }

    public async Task<int> TransactionPaid(string transactionId, DateTime paidTime)
    {
        var transactionModel =
            await _unitOfWorkRepository.TransactionRepository.Where(tr => tr.TransactionCode == transactionId)
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(s => s.Status, PaymentStatus.Paid)
                    .SetProperty(s => s.UpdatedDate, paidTime));
        
        //if (transactionModel == null)
        //{
        //    throw new ModelException("Transaction model", "Not Found", "transaction not exist");
        //}

        await _unitOfWorkRepository.SaveChangesAsync();
        return transactionModel;
    }

    public async Task<GetTransactionDto?> GetTransactionById(int id, GetProfileUserDto user)
    {
        var model = await _unitOfWorkRepository.TransactionRepository.FirstOrDefaultAsync(ts =>
            ts.Id == id && ts.CreatedById == user.Id);
        return model?.Adapt<GetTransactionDto>();
    }

    public async Task<PagedResult<GetTransactionDto>> ViewALlTransaction(TransactionFilterDto transaction, GetProfileUserDto user)
    {
        var listTransactionModel = await _unitOfWorkRepository.TransactionRepository.ViewALlTransaction(transaction, user.Id);
        return listTransactionModel.Adapt<PagedResult<GetTransactionDto>>();
    }
    public async Task<PagedResult<GetTransactionDto>> ViewALlTransactionAsAdmmin(TransactionFilterDto transaction)
    {
        var listTransactionModel = await _unitOfWorkRepository.TransactionRepository.ViewALlTransaction(transaction, 0);
        return listTransactionModel.Adapt<PagedResult<GetTransactionDto>>();
    }
    //public async Task<bool> CreateTransactionForAutoDecreaMoneySlotAsync(int slotId, decimal amount)
    //{
    //    var slotInfor = await _slotStudentServices.GetSlotStudentById(slotId);
    //    TransactionDto transaction = new TransactionDto()
    //    {
    //        TransactionCode = $"AutoPaid_UserId:{slotInfor.UserId}_SlotId:{slotInfor.SlotId}",
    //        Status = PaymentStatus.Paid,
    //        SlotId = slotInfor.SlotId,
    //        CreatedById = slotInfor.UserId,
    //        Amount = amount,
    //        CreatedDate = DateTime.UtcNow,
    //        PaymentMethod = "Internal"
    //    };
    //    var transactionModel = transaction.Adapt<Models.Models.Transaction>();
    //    _unitOfWorkRepository.TransactionRepository.Add(transactionModel);
    //    await _unitOfWorkRepository.SaveChangesAsync();

    //    return true;
    //}

    //public async Task<bool> CreateTransactionForAutoDecreaMoneySlotFailedAsync(int slotId, decimal amount)
    //{
    //    var slotInfor = await _slotStudentServices.GetSlotStudentById(slotId);
    //    TransactionDto transaction = new TransactionDto()
    //    {
    //        TransactionCode = $"AutoPaid_NotSuccess_UserId:{slotInfor.UserId}_SlotId:{slotInfor.SlotId}",
    //        Status = PaymentStatus.Notpaid,
    //        SlotId = slotInfor.SlotId,
    //        CreatedById = slotInfor.UserId,
    //        Amount = amount,
    //        CreatedDate = DateTime.UtcNow,
    //        PaymentMethod = "Internal"
    //    };
    //    var transactionModel = transaction.Adapt<Models.Models.Transaction>();
    //    _unitOfWorkRepository.TransactionRepository.Add(transactionModel);
    //    await _unitOfWorkRepository.SaveChangesAsync();


    //    return true;
    //}

}
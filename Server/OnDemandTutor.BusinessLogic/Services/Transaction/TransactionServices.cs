using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Paging;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.BusinessLogic.Services.Notification;
using OnDemandTutor.Models.Dtos.Notification;

namespace OnDemandTutor.BusinessLogic.Services.Transaction;

public class TransactionServices : ITransactionServices
{
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    public readonly ISlotStudentServices _slotStudentServices;
    private readonly INotificationService _notificationService;

    public TransactionServices(IUnitOfWorkRepository unitOfWorkRepository, INotificationService notificationService, ISlotStudentServices slotStudentServices)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
        _slotStudentServices = slotStudentServices;
        _notificationService = notificationService;
    }

    public async Task<int> CreateTransactionDb(List<TransactionDto> transaction)
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
            ;
        if (transactionModel == null)
        {
            throw new ModelException("Transaction model", "Not Found", "transaction not exist");
        }
        
        await _unitOfWorkRepository.SaveChangesAsync();
        return transactionModel;
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
    public async Task<PagedResult<TransactionDto>> ViewALlTransactionAsAdmmin(TransactionFilterDto transaction)
    {
        var listTransactionModel = await _unitOfWorkRepository.TransactionRepository.ViewALlTransaction(transaction, 0);
        return listTransactionModel.Adapt<PagedResult<TransactionDto>>();
    }
    public async Task<bool> CreateTransactionForAutoDecreaMoneySlotAsync(int slotId, decimal amount)
    {
        var slotInfor = await _slotStudentServices.GetSlotStudentById(slotId);
        TransactionDto transaction = new TransactionDto()
        {
            TransactionCode = $"AutoPaid_UserId:{slotInfor.UserId}_SlotId:{slotInfor.SlotId}",
            Status = PaymentStatus.Paid,
            SlotId = slotInfor.SlotId,
            CreatedById = slotInfor.UserId,
            Amount = amount,
            CreatedDate = DateTime.UtcNow,
            PaymentMethod = "Internal"
        };
        var transactionModel = transaction.Adapt<Models.Models.Transaction>();
        _unitOfWorkRepository.TransactionRepository.Add(transactionModel);
        await _unitOfWorkRepository.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(new NotificationCreateDto()
        {
            Content = $"Giao dịch đã được tạo thành công  ",
            IsViewed = true,
            ReceiverId = transaction.CreatedById,
        });

        return true;
    }

    public async Task<bool> CreateTransactionForAutoDecreaMoneySlotFailedAsync(int slotId, decimal amount)
    {
        var slotInfor = await _slotStudentServices.GetSlotStudentById(slotId);
        TransactionDto transaction = new TransactionDto()
        {
            TransactionCode = $"AutoPaid_NotSuccess_UserId:{slotInfor.UserId}_SlotId:{slotInfor.SlotId}",
            Status = PaymentStatus.Notpaid,
            SlotId = slotInfor.SlotId,
            CreatedById = slotInfor.UserId,
            Amount = amount,
            CreatedDate = DateTime.UtcNow,
            PaymentMethod = "Internal"
        };
        var transactionModel = transaction.Adapt<Models.Models.Transaction>();
        _unitOfWorkRepository.TransactionRepository.Add(transactionModel);
        await _unitOfWorkRepository.SaveChangesAsync();


        await _notificationService.CreateNotificationAsync(new NotificationCreateDto()
        {
            Content = $"Giao dịch đã được tạo thành công  ",
            IsViewed = true,
            ReceiverId = transaction.CreatedById,
        });

        return true;
    }

    public async Task<int> CreateTransactionForClassPayment(string orderId, int userId, int classId, decimal amount)
    {
        var transaction = new Models.Models.Transaction
        {
            TransactionCode = $"Paid for class{classId}",
            CreatedById = userId,
            Amount = amount,
            CreatedDate = DateTime.UtcNow,
            Status = PaymentStatus.Paid,
            TransactionType = TransactionType.Payment,
            UpdatedById = 0,
            PaymentMethod = "VnPay"
        };

        _unitOfWorkRepository.TransactionRepository.Add(transaction);
        await _unitOfWorkRepository.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(new NotificationCreateDto()
        {
            Content = $"Giao dịch đã được tạo thành công  ",
            IsViewed = true,
            ReceiverId = transaction.CreatedById,
        });


        return transaction.Id;
    }

}
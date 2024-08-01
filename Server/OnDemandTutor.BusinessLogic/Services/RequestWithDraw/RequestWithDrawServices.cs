using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.Mail;
using OnDemandTutor.BusinessLogic.Interfaces.RequestWithDraw;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Dtos.WithDrawDto;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Paging;
using System.Security.Claims;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Dtos.User;

namespace OnDemandTutor.BusinessLogic.Services.RequestWithDraw;

public class RequestWithDrawServices : IRequestWithDrawServices
{
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IEmailServices _emailServices;
    private readonly IUserServices _userServices;
    private readonly ITransactionServices _transactionServices;
    private readonly INotificationService _notificationService;

    public RequestWithDrawServices(IUnitOfWorkRepository unitOfWorkRepository, IEmailServices emailServices, IUserServices userServices, 
        INotificationService notificationService,
        ITransactionServices transactionServices)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
        _emailServices = emailServices;
        _userServices = userServices;
        _notificationService = notificationService;
        _transactionServices = transactionServices;
    }

    public async Task<PagedResult<GetRequestWithdrawDto>> ViewAllRequestWithDraw(RequestWithDrawFilterDto request, GetProfileUserDtos user)
    {
        var id = user.Id;
        var requestWithDrawModelList = await
            _unitOfWorkRepository.RequestWithDrawRepository.GetAllRequestWithDraws(request, id);
        return requestWithDrawModelList.Adapt<PagedResult<GetRequestWithdrawDto>>();
    }
    public async Task<PagedResult<GetRequestWithdrawDto>> ViewAllRequestWithDrawAsAdmin(RequestWithDrawFilterDto request)
    {
        var requestWithDrawModelList = await
            _unitOfWorkRepository.RequestWithDrawRepository.GetAllRequestWithDraws(request);
        return requestWithDrawModelList.Adapt<PagedResult<GetRequestWithdrawDto>>();
    }

    public async Task<bool> CreateWithdrawRequest(CreateRequestWithdrawDto request, GetProfileUserDtos user)
    {
        var uid = user.Id;
        var userInfo = await _userServices.GetUserByIdAsync(uid);
        // check money 
        var balanceFromSoureAcc = await _userServices.GetBalanceAsync(userInfo.Id);
        if (balanceFromSoureAcc - request.Amount < 0)
        {
            throw new ModelException("Insufficient balance", "Insufficient balance to make withdraw request",
                "Insufficient balance to make withdraw request");
        }
        // update balance for src acc 
        var requestWithDrawModel = request.Adapt<Models.Models.RequestWithDraw>();
        requestWithDrawModel.UserId = uid;
        requestWithDrawModel.CreatedDate = DateTime.UtcNow;

       var requestWithDrawEntity =   await _unitOfWorkRepository.RequestWithDrawRepository.AddAsync(requestWithDrawModel);
        await _userServices.UpdateBalanceAsync(uid, -request.Amount);
        await _unitOfWorkRepository.SaveChangesAsync();

        // send Email 

        var toAddress = new List<string> { userInfo.Email };
        var emailParams = new Dictionary<string, string>()
        {
            { "UserName", $"{userInfo.FirstName + " " + userInfo.LastName} "},
            { "Amount", $"{request.Amount}"},
            { "BankAccountNumber", $"{request.BankAccountNumber}"},
            { "BankName", $"{request.BankName}"},
            { "Reason", $"{request.Description}" }
        };

        await _emailServices.SendAsync(EmailType.Request_Withdraw_Notification, toAddress, new List<string>(), emailParams,
           false);

        await _notificationService.CreateNotificationAsync(new NotificationCreateDto()
        {
            Content = $"giao dịch với ID = {requestWithDrawEntity.Entity.Id}  đã được tạo ",
            IsViewed = false,
            ReceiverId = new List<int>(userInfo.Id),
        });
        return true;
    }

    public async Task<bool> ApproveWithDraw(ApproveWithDrawDto request, GetProfileUserDtos user)
    {
        var operatorId = user.Id;
        var withdraw = await GetWithdrawRequest(request.Id);

        ValidateWithdrawRequest(withdraw);
        UpdateWithdrawRequest(withdraw, request, operatorId);

        await SendWithdrawApprovalEmail(withdraw, request);

        await CreateTransaction(withdraw, operatorId);
        await _unitOfWorkRepository.SaveChangesAsync();
        
        await _notificationService.CreateNotificationAsync(new NotificationCreateDto()
        {
            Content = $" đon rút tiền với Id{withdraw.Id}  đã được xử li  ",
            IsViewed = false,
            ReceiverId = new List<int>(withdraw.UserId),
        });
        return true;
    }


    private async Task<Models.Models.RequestWithDraw> GetWithdrawRequest(int requestId)
    {
        return await _unitOfWorkRepository.RequestWithDrawRepository.FirstOrDefaultAsync(rw => rw.Id == requestId);
    }

    private void ValidateWithdrawRequest(Models.Models.RequestWithDraw withdraw)
    {
        if (withdraw == null || withdraw.Status != WithDrawStatus.Pending)
        {
            throw new ModelException("Invalid Request Withdraw", "Invalid Request Withdraw, please check your request",
                "Invalid Request Withdraw");
        }
    }

    private void UpdateWithdrawRequest(Models.Models.RequestWithDraw withdraw, ApproveWithDrawDto request, int operatorId)
    {
        withdraw.Status = request.Status;
        withdraw.Reply = request.Reply;
        withdraw.OperatorId = operatorId;
        withdraw.UpdatedDate = DateTime.Now;
        withdraw.UpdatedById = operatorId;
        _unitOfWorkRepository.RequestWithDrawRepository.Update(withdraw);
        _unitOfWorkRepository.SaveChanges();
    }

    private async Task SendWithdrawApprovalEmail(Models.Models.RequestWithDraw withdraw, ApproveWithDrawDto request)
    {
        var withDrawCreatedBy = await _userServices.GetUserByIdAsync(withdraw.UserId);
        var toAddress = new List<string> { withDrawCreatedBy.Email };
        var emailParams = new Dictionary<string, string>
            {
                { "UserName", $"{withDrawCreatedBy.FirstName} {withDrawCreatedBy.LastName}" },
                { "Status", request.Status.ToString() },
                { "Reply", request.Reply },
                { "Amount", withdraw.Amount.ToString() }
            };
        await _emailServices.SendAsync(EmailType.WithDraw_Approval_Notification, toAddress, new List<string>(), emailParams, false);
    }

    private async Task CreateTransaction(Models.Models.RequestWithDraw withdraw, int operatorId)
    {
        var transaction = new TransactionDto
        {
            TransactionCode = DateTime.Now.Ticks.ToString() + "request_withdraw",
            Amount = withdraw.Amount,
            PaymentMethod = "Bank transfer",
            Notes = $"Request withdrawal #{withdraw.Id}",
            Status = PaymentStatus.Paid,
            CreatedDate = DateTime.Now,
            CreatedById = withdraw.UserId,
            TransactionType = TransactionType.WithDraw
        };

        await _transactionServices.CreateTransactionDb(new List<TransactionDto> { transaction });
        _unitOfWorkRepository.SaveChanges();
    }

}
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OnDemandTutor.BusinessLogic.Interfaces.Class;
using OnDemandTutor.BusinessLogic.Interfaces.Payment;
using OnDemandTutor.BusinessLogic.Interfaces.Slot;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Interfaces.StudentClass;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.Repository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Dtos.Payment;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.BusinessLogic.Services.Payment;

public class VnPayServices : IVnPayServices
{
    private readonly VnPay _vnPay;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly ITransactionServices _transactionServices;
    private readonly ISlotStudentServices _slotStudentServices;
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly IUserServices _userServices;
    private readonly IClassServices _classServices;
    private readonly ISlotServices _slotServices;
    private readonly IStudentClassService _studentClassService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VnPayServices(IOptions<VnPay> vnPay, IConfiguration configuration,
        IUnitOfWorkRepository unitOfWorkRepository, ITransactionServices transactionServices,
        ISlotStudentServices slotStudentServices, IPaymentProcessor paymentProcessor,
        IStudentClassService studentClassService, ISlotServices slotServices,
        IClassServices classServices, IHttpContextAccessor httpContextAccessor,
        IUserServices userServices)
    {
        _vnPay = vnPay.Value;
        _configuration = configuration;
        _unitOfWorkRepository = unitOfWorkRepository;
        _transactionServices = transactionServices;
        _slotStudentServices = slotStudentServices;
        _paymentProcessor = paymentProcessor;
        _userServices = userServices;
        _classServices = classServices;
        _slotServices = slotServices;
        _studentClassService = studentClassService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> CreatePaymentForSlotUrl(PaySlotDto model, HttpContext context, GetSlotsDtos slot)
    {
        List<int> listSlotId = new List<int> { slot.Id };
        var tick = DateTime.Now.Ticks.ToString();

        var tutor = await _userServices.GetProfileAsync(slot.CreateById, null, null);
        var slotCost = (tutor.TutorFeePerHour * (decimal)(slot.EndTime - slot.StartTime).TotalHours);

        var paymentUrl = CreateVnPayRequest(model, context, new List<int> { slot.Id }, null, slotCost, model.OrderDescription, false, tick, model.ReturnUrl);

        var transactionDto = CreateTransactionDto(tick, "Vnpay-bankcode", slotCost, model.OrderDescription, listSlotId, null, context, TransactionType.Payment);
        await _transactionServices.CreateTransactionDb(transactionDto);


        return paymentUrl;
    }

    public async Task<PaymentSlotResponseModel> PaymentExecute(IQueryCollection collections)
    {
        var response = _paymentProcessor.ProcessPaymentResponse(collections);

        if (response.Success)
        {
            await _transactionServices.TransactionPaid(response.OrderId, DateTime.UtcNow);

            if (response.SlotId != null && response.ClassId == null && response.SlotId.Count == 1)
            {
                await HandleSingleSlotPayment(response);
            }
            else if (response.ClassId != null)
            {
                await HandleClassPayment(response);
            }
            else
            {
                await _userServices.RechargeAccountAsync(response.UserId, response.Money);
            }

            await _userServices.UpdateBalanceAsync(response.UserId, 0, response.Money);

            if (response.VnPayResponseCode == "24")
            {
                return CreatePaymentResponseModel(response, false, PaymentStatus.Notpaid, "khong thanh cong");
            }
        }

        return CreatePaymentResponseModel(response, true, PaymentStatus.Paid);
    }


    private async Task HandleSingleSlotPayment(IPaymentResponse response)
    {
        var slotStudent = await _slotStudentServices.GetSlotStudentAsync(response.SlotId.First(), response.UserId);
        if (slotStudent == null)
        {
            await _slotServices.EnrollForSlot(response.UserId, response.SlotId.First());
        }
        await _slotStudentServices.SlotStudentPaidAsync(response.SlotId.First(), response.UserId);
    }

    private async Task HandleClassPayment(IPaymentResponse response)
    {
        var slotInClass = await _classServices.GetClassByIdAsync(response.ClassId.Value);
        
        await _transactionServices.CreateTransactionForClassPayment(response.OrderId, response.UserId, response.ClassId.Value, response.Money);

        await _classServices.EnrollCLass(response.ClassId.Value, response.UserId);
    
        foreach (var slot in slotInClass.Slots)
        {
            await _slotStudentServices.CreateSlotStudentIfNotExist(slot.Id, response.UserId);
            await _slotStudentServices.SlotStudentPaidAsync(slot.Id, response.UserId);
        }
    }


    private PaymentSlotResponseModel CreatePaymentResponseModel(IPaymentResponse response, bool success, PaymentStatus paymentStatus, string additionalDescription = "")
    {
        return new PaymentSlotResponseModel
        {
            PaymentStatus = paymentStatus,
            SlotId = response.SlotId,
            TransactionCode = response.OrderId,
            UserId = response.UserId,
            TransactionId = 0,
            Money = response.Money,
            Success = success,
            PaymentMethod = response.PaymentMethod,
            VnPayResponseCode = response.VnPayResponseCode,
            IsRechargePayment = response.IsRechargePayment,
            RedirectResult = response.returnUrl,
            OrderDescription = response.OrderDescription + additionalDescription
        };
    }


    public async Task<string> RechargePaymentAsync(RechargeDto model, HttpContext context)
    {
        var tick = DateTime.Now.Ticks.ToString();
        var paymentUrl = CreateVnPayRequest(model, context, null, null, model.Amount, model.Notes, true, tick, model.returnUrl);

        var transactionDto = CreateTransactionDto(tick, "Vnpay-bankcode", model.Amount, model.Notes, null, null, context, TransactionType.Recharge);
        await _transactionServices.CreateTransactionDb(transactionDto);
        return paymentUrl;
    }

    public async Task<string> CreatePaymentForClassUrl(PayClassDto model, HttpContext context, GetClassFullDataSlotDto classDto)
    {
        var userId = context.User.FindFirst(cl => cl.Type == "id")?.Value;
        var tutorPriceInCurr = await _userServices.GetUserProfileByIdAsync(classDto.TutorId);
        var tutorFee = tutorPriceInCurr.TutorFeePerHour;

        double totalHoursNotYet = 0;
        List<int> slotIds = new List<int>();
        foreach (var slot in classDto.Slots)
        {
            if (slot.SlotStatus == SlotStatus.NotYet && slot.PaymentStatus == PaymentStatus.Notpaid)
            {
                var totalTime = slot.EndTime - slot.StartTime;
                totalHoursNotYet += totalTime.TotalHours;
                slotIds.Add(slot.Id);
            }
        }
        var totalAmount = (decimal)(tutorFee * (decimal)totalHoursNotYet)!;
        if (!model.IsFullPay)
        {
            totalAmount *= 0.2m; // 20% of the total amount
        }

        var tick = DateTime.Now.Ticks.ToString();

        var paymentUrl = CreateVnPayRequest(model, context, slotIds, classDto.Id, totalAmount, model.OrderDescription, false, tick, model.returnPage);

        var transactionDto = CreateTransactionDto(tick, "Vnpay-bankcode", totalAmount, model.OrderDescription, slotIds, classDto.Id, context);

        await _transactionServices.CreateTransactionDb(transactionDto);

        return paymentUrl;
    }

    public async Task<bool> CreatePaymentForSlotByUserBalance(PaySlotDto model, HttpContext context, GetSlotsDtos slot)
    {
        var userId = context.User.FindFirst(c => c.Type == "id")?.Value;
        var listSlotId = new List<int>();
        listSlotId.Add(slot.Id);
        var tick = DateTime.Now.Ticks.ToString();

        var tutor = await _userServices.GetProfileAsync(slot.CreateById, null, null);
        decimal slotCost = (tutor.TutorFeePerHour * (decimal)(slot.EndTime - slot.StartTime).TotalHours);

        var transactionDto = CreateTransactionDto(tick, "User-Balance", slotCost, model.OrderDescription, listSlotId, null, context, TransactionType.Payment);
        await _transactionServices.CreateTransactionDb(transactionDto);
        var studentModel = await _userServices.GetProfileAsync(int.Parse(userId), null, null);
        await _slotStudentServices.CreateSlotStudentIfNotExist(slot.Id, studentModel.Id);
        var slotStudentDto = await _slotStudentServices.GetSlotStudentAsync(slot.Id, studentModel.Id);
        var balance = await _userServices.GetBalanceAsync(studentModel.Id);
        if (balance > slotCost && slotStudentDto.PaymentStatus == PaymentStatus.Notpaid)
        {
            await _transactionServices.TransactionPaid(tick, DateTime.Now);
            await _userServices.UpdateBalanceAsync(studentModel.Id, 0, slotCost);
            await _slotStudentServices.SlotStudentPaidAsync(slot.Id, studentModel.Id);
            return true;

        }

        return false;

    }


    private string CreateVnPayRequest<T>(T model, HttpContext context, List<int>? slotId, int? classId, decimal amount, string? description, bool? isRechargePayment, string tick, string? returnPage)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var pay = new VnPayLibrary();

        var currUid = context.User.FindFirst(c => c.Type == "id")?.Value;
        string slotIdString = slotId != null ? string.Join(" ", slotId) : string.Empty;
        pay.AddRequestData("vnp_Version", _vnPay.Version);
        pay.AddRequestData("vnp_Command", _vnPay.Command);
        pay.AddRequestData("vnp_TmnCode", _vnPay.TmnCode);
        pay.AddRequestData("vnp_Amount", ((int)amount * 100).ToString());
        pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", _vnPay.CurrCode);
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale", _vnPay.Locale);
        pay.AddRequestData("vnp_OrderInfo", $"{isRechargePayment}|{description}|{currUid}|{classId}|{returnPage}|{slotIdString}");
        pay.AddRequestData("vnp_OrderType", "other");
        pay.AddRequestData("vnp_ReturnUrl", "https://localhost:7142/api/Payment/execute");
        pay.AddRequestData("vnp_TxnRef", tick);
        pay.AddRequestData("vnp_ExpireDate", timeNow.AddMinutes(20).ToString("yyyyMMddHHmmss"));

        var paymentUrl = pay.CreateRequestUrl(_vnPay.BaseUrl, _vnPay.HashSecret);
        return paymentUrl;
    }

    private List<TransactionDto> CreateTransactionDto(string tick, string paymentMethod, decimal amount, string? notes, List<int> slotIds, int classId, HttpContext context)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var currUid = context.User.FindFirst(c => c.Type == "id")?.Value;

        var transactionDtos = new List<TransactionDto>();

        foreach (var slotId in slotIds)
        {
            var transactionDto = new TransactionDto
            {
                TransactionCode = tick,
                PaymentMethod = paymentMethod,
                Amount = amount,
                Notes = notes,
                SlotId = slotId,
                ClassId = classId,
                Status = PaymentStatus.Notpaid,
                CreatedDate = DateTime.Now,
                CreatedById = int.Parse(currUid),
            };

            transactionDtos.Add(transactionDto);
        }

        return transactionDtos;
    }
    private List<TransactionDto> CreateTransactionDto(string tick, string paymentMethod, decimal amount, string? notes,
        List<int> slotIds, int? classId, HttpContext context, TransactionType transactionType)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var currUid = context.User.FindFirst(c => c.Type == "id")?.Value;

        var transactionDtos = new List<TransactionDto>();

        foreach (var slotId in slotIds)
        {
            var transactionDto = new TransactionDto
            {
                TransactionCode = tick,
                PaymentMethod = paymentMethod,
                Amount = amount,
                Notes = notes,
                SlotId = slotId,
                ClassId = classId,
                Status = PaymentStatus.Notpaid,
                CreatedDate = timeNow,
                CreatedById = int.Parse(currUid),
                TransactionType = transactionType
            };

            transactionDtos.Add(transactionDto);
        }

        return transactionDtos;
    }

}

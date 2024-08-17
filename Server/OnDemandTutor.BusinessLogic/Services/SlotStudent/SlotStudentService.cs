using Mapster;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.BusinessLogic.Services.Slot;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.SlotStudent;
using OnDemandTutor.Models.Dtos.StudentSlot;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;
using System.Globalization;

namespace OnDemandTutor.BusinessLogic.Services.SlotStudent;

public class SlotStudentService : ISlotStudentServices
{
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IUserServices _userServices;
    private readonly INotificationService _notificationService;
    private readonly ITransactionServices _transactionServices;


    public SlotStudentService(IUnitOfWorkRepository unitOfWorkRepository, INotificationService notificationService, IUserServices userServices, ITransactionServices transactionServices)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
        _userServices = userServices;
        _notificationService = notificationService;
        _transactionServices = transactionServices;

    }
    public async Task<List<GetSlotStudentDetailDto>> QuerySlotStudent(QuerySlotStudentDto querySlotStudentDto, GetProfileUserDtos? user)
    {
        var slotStudent =
            await _unitOfWorkRepository.SlotStudentRepository.GetStudentSlotsAsync(querySlotStudentDto, user?.Id);
        return slotStudent.Adapt<List<GetSlotStudentDetailDto>>();
    }

    public async Task<List<GetSlotStudentDetailDto>> GetSimpleStudentSlotOfStudent(int studentId)
    {
        return (await _unitOfWorkRepository.SlotStudentRepository.GetSlotOfStudent(studentId)).Adapt<List<GetSlotStudentDetailDto>>();
    }
    public async Task<PagedResult<GetSlotStudentDetailDto>> GetStudentSlotByTutor(PagingModel<QueryRatingDto> queryRatingDto)
    {
        var slotStudent =
            await _unitOfWorkRepository.SlotStudentRepository.GetStudentSlotByTutor(queryRatingDto);
        return slotStudent.Adapt<PagedResult<GetSlotStudentDetailDto>>();
    }
    public async Task<GetSlotStudentDetailDto> GetClosestFutureSlot(GetProfileUserDtos user)
    {
        var slotStudent =
            await _unitOfWorkRepository.SlotStudentRepository.GetClosestFutureSlot(user.Id);
        return slotStudent.Adapt<GetSlotStudentDetailDto>();
    }
    public async Task<SlotStudentDto> GetSlotStudentAsync(int slotId, int studentId)
    {
        var slotStudent =
            await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(st =>
                st.SlotId == slotId && st.UserId == studentId);
        return slotStudent.Adapt<SlotStudentDto>();
    }

    public async Task<PagedResult<GetSlotStudentWithDetailStudentDto>> GetSlotStudentsOfSlotPaged(int slotId, int page, int limit)
    {
        var slotStudents = await _unitOfWorkRepository.SlotStudentRepository.GetStudentsSlotWithStudentBySlotIdPaged(slotId, page, limit);
        return slotStudents.Adapt<PagedResult<GetSlotStudentWithDetailStudentDto>>();
    }
    public async Task<List<GetSlotStudentWithDetailStudentDto>> GetSlotStudentsOfSlotAsync(int slotId)
    {
        var slotStudents = await _unitOfWorkRepository.SlotStudentRepository.GetStudentsSlotWithStudentBySlotId(slotId);
        return slotStudents.Adapt<List<GetSlotStudentWithDetailStudentDto>>();
    }
    public async Task<bool> SlotStudentPaidAsync(int slotId, int studentId, decimal value)
    {
        var slotStudent =
            await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(st =>
                st.SlotId == slotId && st.UserId == studentId);
        if (slotStudent.PaymentStatus == PaymentStatus.Paid)
        {
            throw new Exception($"this course has already paid by studentId {studentId}");
        }

        slotStudent.PaymentStatus = PaymentStatus.Paid;
        slotStudent.PaidValue = value;
        _unitOfWorkRepository.SlotStudentRepository.Update(slotStudent);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }
    public async Task<SlotStudentDto> GetSlotStudentById(int slotId)
    {
        var recordInDb = await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(u => u.SlotId == slotId);
        return recordInDb.Adapt<SlotStudentDto>();
    }

    public async Task<List<GetStudentSlotDto>> GetListSLotStudentByStatus(PaymentStatus status)
    {
        var slotStudentModel = await _unitOfWorkRepository.SlotStudentRepository.Where(ss => ss.PaymentStatus == status).ToListAsync();
        return slotStudentModel.Adapt<List<GetStudentSlotDto>>();
    }
    public async Task<bool> SoftDeleteSlotStudent(int slotId, int studentId)
    {
        var slotstudent = await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(sc => sc.SlotId == slotId && sc.UserId == studentId);
        if (slotstudent == null)
        {
            throw new NotFoundException("Slot Student not found");
        }

        _unitOfWorkRepository.SlotStudentRepository.Remove(slotstudent);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    public async Task<List<SlotStudentDto>> GetListSlotStudentByStudentId(int studentId)
    {
        var slotStudentModel = await _unitOfWorkRepository.SlotStudentRepository.Where(ld => ld.UserId == studentId).ToListAsync();
        return slotStudentModel.Adapt<List<SlotStudentDto>>();
    }

    public async Task<bool> CreateSlotStudentIfNotExists(int slotId, int studentId)
    {
        var existSlotStudent = await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(ss => ss.SlotId == slotId && ss.UserId == studentId);
        if (existSlotStudent == null)
        {
            var newSlotStudentModel = new Models.Models.SlotStudent()
            {
                UserId = studentId,
                SlotId = slotId,
                PaymentStatus = PaymentStatus.Notpaid,
                PaidValue = 0,
                IsTransferred = false,
            };
            await _unitOfWorkRepository.SlotStudentRepository.AddAsync(newSlotStudentModel);
            await _unitOfWorkRepository.SaveChangesAsync();
        }   
        return true;
    }
    public async Task<bool> UpdateSlotStudentAsync(int slotId, int studentId, decimal rate, string feedback)
    {
        var slotStudent = await _unitOfWorkRepository.SlotStudentRepository.GetSlotStudentBySlotIdAndStudentId(slotId, studentId);

        if (slotStudent == null)
        {
            throw new NotFoundException("Slot Student not found");
        }

        slotStudent.Rating = rate;
        slotStudent.Feedback = feedback;

        await _userServices.RecalculateTutorRating(slotStudent.Slot.CreateById);

        _unitOfWorkRepository.SlotStudentRepository.Update(slotStudent);
        await _unitOfWorkRepository.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(new CreateNotificationDto
        {
            Content = $"Bạn đã nhận được 1 đánh giá về buổi học {slotStudent.Slot.StartTime} đến {slotStudent.Slot.EndTime} từ học sinh {slotStudent.User.FirstName} {slotStudent.User.LastName}",
            ReceiverIds = new List<int> { slotStudent.Slot.CreateById },
            RefImageUrl = slotStudent.User.AvatarImageUrl,
            RefUrl = "/tutor/profile"
        });

        return true;
    }


    public async Task CronJobForAutoDereasedMoneyAfterSlotStart()
    {
        var slotStudents = await _unitOfWorkRepository.SlotStudentRepository.GetAboutToStartStudentSlots();
        foreach (var slotStudent in slotStudents) 
        {
            var tutor = slotStudent.Slot.CreatedBy;
            var duration = (slotStudent.Slot.EndTime - slotStudent.Slot.StartTime).TotalHours;

            var studentBalance = await _userServices.GetBalanceAsync(slotStudent.UserId);
            var amountToDecrease = (tutor.TutorFeePerHour ?? 0) * (decimal)duration;
            decimal slotCost = (tutor.TutorFeePerHour ?? 0) * (decimal)duration;
            if (studentBalance - slotCost >= 0)
            {
                await _userServices.UpdateBalanceAsync(slotStudent.UserId, -slotCost);
                await _transactionServices.CreateTransactionDb(new List<Models.Dtos.Transaction.TransactionDto>
                {
                    new Models.Dtos.Transaction.TransactionDto
                    {
                        TransactionCode = DateTime.Now.Ticks + "_" + slotStudent.UserId,
                        Notes = $"AutoPaid_UserId:{slotStudent.UserId}_SlotId:{slotStudent.SlotId}",
                        Status = PaymentStatus.Paid,
                        SlotId = slotStudent.SlotId,
                        CreatedById = slotStudent.UserId,
                        Amount = amountToDecrease,
                        CreatedDate = DateTime.UtcNow,
                        PaymentMethod = "Internal",
                        TransactionType = TransactionType.Deduction
                    }
                });
                await _notificationService.CreateNotificationAsync(new CreateNotificationDto
                {
                    Content = $"Hệ thống đã tự quét trừ {amountToDecrease.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"))} từ số dư tài khoản để trả cho Slot học sắp tới của bạn." +
                    $"Chúc bạn và gia sư có 1 buổi học thành công tốt đẹp.",
                    ReceiverIds = new List<int> { slotStudent.UserId },
                    RefImageUrl = tutor.AvatarImageUrl,
                    RefUrl = "/student/schedule"
                    
                });
                await SlotStudentPaidAsync(slotStudent.SlotId, slotStudent.UserId, amountToDecrease);
            }           
        }
        
    }

    public async Task LeaveSlot(int slotId, GetProfileUserDtos user)
    {
        var slotStudent = await _unitOfWorkRepository.SlotStudentRepository.GetSlotStudentBySlotIdAndStudentId(slotId, user.Id);
        if (slotStudent == null)
        {
            throw new NotFoundException("Slot of this student is not found");
        }
        if (slotStudent.Slot.SlotStatus == SlotStatus.OnGoing || slotStudent.Slot.SlotStatus == SlotStatus.Finished)
        {
            throw new BadRequestException("You can only leave the slot that has not started yet!");
        }
        if (slotStudent.Slot.SlotStatus  == SlotStatus.Cancelled && slotStudent.PaymentStatus == PaymentStatus.Paid)
        {
            await Refund(slotId, user.Id);
        }
        await SoftDeleteSlotStudent(slotId, user.Id);
    }

    public async Task Refund(int slotId, int userId)
    {
        var slotStudent = await _unitOfWorkRepository.SlotStudentRepository.GetSlotStudentBySlotIdAndStudentId(slotId, userId);
        if (slotStudent == null)
        {
            throw new NotFoundException("Slot of this student is not found");
        }
        //Ignore this check if the isCheckStatus is false
        if (slotStudent.Slot.SlotStatus != SlotStatus.Cancelled || slotStudent.PaymentStatus == PaymentStatus.Notpaid)
        {
            throw new BadRequestException("Slot must be paid or slot is cancelled in order to refund");
        }

        var tutor = await _userServices.GetProfileAsync(slotStudent.Slot.CreateById, null, null);
        var cost = tutor.TutorFeePerHour * (decimal)(slotStudent.Slot.EndTime - slotStudent.Slot.StartTime).TotalHours;
        await _userServices.UpdateBalanceAsync(userId, cost);
    }

    public async Task SetTransferred(int id)
    {
        var slotStudent = await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(ss => ss.Id == id);
        if (slotStudent == null)
        {
            throw new NotFoundException("Slot student not found");
        }
        slotStudent.IsTransferred = true;
        _unitOfWorkRepository.SlotStudentRepository.Update(slotStudent);
        await _unitOfWorkRepository.SaveChangesAsync();
    }
}
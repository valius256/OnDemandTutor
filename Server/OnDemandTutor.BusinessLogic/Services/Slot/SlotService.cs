using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Class;
using OnDemandTutor.BusinessLogic.Interfaces.Mail;
using OnDemandTutor.BusinessLogic.Interfaces.Slot;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Interfaces.StudentClass;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.Slot
{
    public class SlotService : ISlotServices
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly ISlotRepository _slotRepository;
        private readonly IAuthServices _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISlotStudentServices _slotStudentServices;
        private readonly IUserServices _userServices;
        private readonly ITransactionServices _transactionServices;
        private readonly IClassService _classService;
        private readonly IMailServices _mailServices;
        private readonly IStudentClassService _studentClassService;

        public SlotService(IUnitOfWorkRepository unitOfWorkRepository,
            ISlotStudentServices slotStudentServices, ITransactionServices transactionServices, IUserServices userServices,
            IClassService classService, IMailServices mailServices, IStudentClassService studentClassService,
            ISlotRepository slotRepository, IAuthServices authService, IHttpContextAccessor HttpContextAccessor)
        {
            _unitOfWork = unitOfWorkRepository;
            _slotRepository = slotRepository;
            _authService = authService;
            _httpContextAccessor = HttpContextAccessor;
            _slotStudentServices = slotStudentServices;
            _transactionServices = transactionServices;
            _userServices = userServices;
            _classService = classService;
            _mailServices = mailServices;
            _studentClassService = studentClassService;
        }


        public async Task<PagedResult<GetSlotsDtos>> GetSlotsAsync(PagingModel<GetSlotsDtos> request)
        {
            return await _unitOfWork.SlotRepository.GetSlotsAsync(request);
        }

        public async Task<GetSlotsDtos> GetSlotByIdAsync(int id)
        {
            var slot = await _unitOfWork.SlotRepository.GetSlotByIdAsync(id);
            if (slot is null)
            {
                throw new BadRequestException("Slot not found");
            }
            return slot;
        }

        public async Task<CreateSlotsDtos> CreateSlotAsync(CreateSlotsDtos slotDto)
        {

            var slotEntity = slotDto.Adapt<CreateSlotsDtos>(); // Assuming Mapster is used for mapping

            // Add the new Slot entity to repository
            var createdSlotEntity = await _unitOfWork.SlotRepository.CreateSlotAsync(slotEntity);
            await _unitOfWork.SaveChangesAsync();

            // Map the created entity back to CreateSlotsDtos and return it
            var createdSlotDto = createdSlotEntity.Adapt<CreateSlotsDtos>(); // Mapster mapping

            return createdSlotDto;
        }
        public async Task<UpdateSlotDtos> UpdateSlotAsync(UpdateSlotDtos slotDto)
        {
            // Retrieve the existing slot entity from the database
            var existingSlotEntity = await _unitOfWork.SlotRepository.FirstOrDefaultAsync(s => s.Id == slotDto.Id);

            // Check if the entity is null
            if (existingSlotEntity == null)
            {
                throw new NotFoundException($"Slot with ID {slotDto.Id} not found.");
            }

            // Get the current user from the authentication service
            var user = await _authService.GetUserProfileByClaim(_httpContextAccessor.HttpContext.User);

            // Adapt the incoming DTO to the existing entity
            existingSlotEntity = slotDto.Adapt(existingSlotEntity);

            // Set the updated fields
            existingSlotEntity.UpdatedById = user.Id; // Assuming there is an UpdatedById property
            existingSlotEntity.UpdatedDate = DateTime.Now; // Assuming there is an UpdatedDate property

            // Update the entity in the database
            var updatedSlotEntity = _unitOfWork.SlotRepository.Update(existingSlotEntity);

            // Save the changes
            await _unitOfWork.SaveChangesAsync();

            // Return the updated DTO
            return updatedSlotEntity.Entity.Adapt<UpdateSlotDtos>();
        }


        public async Task<bool> DeleteSlotAsync(int id)
        {
            var isDeleted = await _unitOfWork.SlotRepository.DeleteSlotAsync(id);
            await _unitOfWork.SaveChangesAsync();
            if (!isDeleted)
            {
                throw new NotFoundException($"Slot with ID {id} not found.");
            }
            return isDeleted;
        }

        public async Task CronJobForAutoDereasedMoneyAfterSlotStart()
        {
            var listSlotDb = await _unitOfWork.SlotRepository.ToListAsync();

            var slots = listSlotDb
                .Where(slot => slot.StartTime >= DateTime.Now.AddHours(-1)).ToList();
            foreach (var slot in slots)
            {
                var slotStudent = await _slotStudentServices.GetSlotStudentById(slot.Id);
                if (slotStudent.PaymentStatus == PaymentStatus.Notpaid)
                {
                    var tutor = await _userServices.GetProfile(slot.CreateById, null, null);
                    var duration = (slot.EndTime - slot.StartTime).TotalHours;

                    var studentBalance = await _userServices.GetBalanceAsync(slotStudent.UserId);
                    var amountToDecrease = tutor.TutorFeePerHour * (decimal)duration;
                    decimal slotCost = tutor.TutorFeePerHour * (decimal)duration;
                    if (studentBalance - slotCost >= 0)
                    {
                        await _userServices.UpdateBalance(slotStudent.UserId, 0, slotCost);
                        await _transactionServices.CreateTransactionForAutoDecreaMoneySlotAsync(slot.Id, -amountToDecrease);
                        await _slotStudentServices.SlotStudentPaidAsync(slot.Id, slotStudent.UserId);
                    }
                    else
                    {
                        await _transactionServices.CreateTransactionForAutoDecreaMoneySlotFailedAsync(slot.Id,
                            -amountToDecrease);
                    }
                }
                else
                {
                    return;
                }
            }
        }

        public async Task CronJobForAutoCheckIfStudentDeptIsMoreThan20Percent()
        {
            var listOfNotPaidSlotStudent = await _slotStudentServices.GetListSLotStudentByStatus(PaymentStatus.Notpaid);
            var listOfSlotIds = listOfNotPaidSlotStudent.Select(l => l.SlotId).Distinct().ToList();

            foreach (var slotId in listOfSlotIds)
            {
                var listOfSlotTotal = await GetListOfSlotSameClassBySlotId(slotId);
                var countListNotPaid = listOfSlotTotal
                    .SelectMany(ls => ls.SlotStudents)
                    .Count(ss => ss.PaymentStatus == PaymentStatus.Notpaid);

                var totalSlot = listOfSlotTotal.Count;

                if (totalSlot == 0) continue;

                double percentageNotPaid = (double)countListNotPaid / totalSlot;
                
                if (percentageNotPaid >= 0.20)
                {
                    var slot = await _slotStudentServices.GetSlotStudentById(slotId);
                    var user = await _userServices.GetProfile(slot.UserId, null, null);
                    // Save this email as a template or send directly
                    var emailParams = new Dictionary<string, string>()
                    {
                        { "Name", $"{user.FirstName}" },
                        { "ClassId", $"{listOfSlotTotal.FirstOrDefault()?.ClassId}"},
                    };

                    List<string> toAddress = new List<string> { user.Email };
                    await _mailServices.SendAsync(EmailType.Remove_Unpaid_Slots, toAddress, new List<string> { }, emailParams, false);
                    var slotModel = listOfSlotTotal.Adapt<Models.Models.Slot>();
                    var classId = slotModel.ClassId;

                    await _studentClassService.DeleteStudentFromStudentClassById(user.Id, classId.Value);
                }
                else if (percentageNotPaid >= 0.15)
                {
                    var slot = await _slotStudentServices.GetSlotStudentById(slotId);
                    var user = await _userServices.GetProfile(slot.UserId, null, null);
                    // Save this email as a template or send directly
                    var emailParams = new Dictionary<string, string>()
                    {
                        { "Name", $"{user.FirstName}" },
                        { "ClassId", $"{listOfSlotTotal.FirstOrDefault().ClassId}"},
                    };

                    List<string> toAddress = new List<string> { user.Email };


                    await _mailServices.SendAsync(EmailType.Slot_Payment_Reminder, toAddress, new List<string> { }, emailParams, false);
                }
            }
        }

        public async Task<List<GetSlotWithSlotStudentDto>> GetListOfSlotSameClassBySlotId(int slotId)
        {
            var classId = await _slotRepository.Where(sl => sl.Id == slotId).Select(l => l.ClassId).FirstOrDefaultAsync();
            var listSlotWithSameClass = await _slotRepository.Where(sl => sl.ClassId == classId).ToListAsync();
            return listSlotWithSameClass.Adapt<List<GetSlotWithSlotStudentDto>>();
        }
    }
}


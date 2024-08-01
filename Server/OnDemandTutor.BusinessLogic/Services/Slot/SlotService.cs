using Mapster;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Mail;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.BusinessLogic.Interfaces.Slot;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Interfaces.StudentClass;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.Slot
{
    public class SlotService : ISlotServices
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly ISlotRepository _slotRepository;
        private readonly IAuthServices _authService;
        private readonly ISlotStudentServices _slotStudentServices;
        private readonly IUserServices _userServices;
        private readonly ITransactionServices _transactionServices;
        private readonly IEmailServices _emailServices;
        private readonly IStudentClassService _studentClassService;
        private readonly INotificationService _notificationService;

        public SlotService(IUnitOfWorkRepository unitOfWorkRepository,
            ISlotStudentServices slotStudentServices, ITransactionServices transactionServices, IUserServices userServices,
            IEmailServices emailServices, IStudentClassService studentClassService, INotificationService notificationService,
            ISlotRepository slotRepository, IAuthServices authService)
        {
            _unitOfWork = unitOfWorkRepository;
            _slotRepository = slotRepository;
            _authService = authService;
            _slotStudentServices = slotStudentServices;
            _transactionServices = transactionServices;
            _userServices = userServices;
            _emailServices = emailServices;
            _studentClassService = studentClassService;
            _notificationService = notificationService;
        }


        public async Task<PagedResult<GetSlotsDtos>> GetSlotsAsync(PagingModel<QuerySlotDto> request)
        {
            return await _unitOfWork.SlotRepository.GetSlotsAsync(request);
        }
        public async Task<GetSlotsDtos> GetClosestSlotOfTutor(GetProfileUserDtos tutor)
        {
            if (tutor.Role != RoleStatus.Tutor)
            {
                throw new BadRequestException("This User is not a tutor");
            }
            return (await _unitOfWork.SlotRepository.GetClosestFutureSlotOfTutor(tutor.Id)).Adapt<GetSlotsDtos>();
        }
        public async Task<GetSlotDetailDto> GetSlotByIdAsync(int id)
        {
            var slot = await _unitOfWork.SlotRepository.GetSlotByIdAsync(id);
            if (slot is null)
            {
                throw new NotFoundException("Slot not found");
            }
            return slot;
        }
        private async Task ValidateSlot(Models.Models.Slot slot)
        {
            if (slot.StartTime >= slot.EndTime)
            {
                throw new BadRequestException($"Start time must be smaller than end time");
            }
            if (slot.StartTime <= DateTime.Now)
            {
                throw new BadRequestException($"Start time must be in the future");
            }
            var duration = slot.EndTime - slot.StartTime;
            if (duration.TotalMinutes < 15)
            {
                throw new BadRequestException($"Slot duration is minimum 15 minutes");
            }
            if (duration.TotalHours > 4)
            {
                throw new BadRequestException($"Slot duration is maximum 4 hours");
            }
            var tutorAllSlot = await _slotRepository.Where(sl => sl.CreateById == slot.CreateById).ToListAsync();
            foreach (var existSlot in tutorAllSlot)
            {
                if (slot.Id == existSlot.Id) continue;
                if (slot.StartTime <= existSlot.EndTime && slot.EndTime >= existSlot.StartTime)
                {
                    throw new BadRequestException($"There is a schedule conflict with slot [Start : {existSlot.StartTime}; End : {existSlot.EndTime}], please check again");
                }
            }
        }
        public async Task ValidateSlotForStudent(int slotId, int studentId)
        {
            var slot = await _slotRepository.FirstOrDefaultAsync(s => s.Id == slotId);
            if (slot == null)
            {
                throw new NotFoundException("Slot not found");
            }
            var listOfStudentSlots = await _slotStudentServices.GetSimpleStudentSlotOfStudent(studentId);

            foreach (var studentSlot in listOfStudentSlots)
            {
                if (studentSlot.Slot.Id == slotId) continue;
                // Check if the slot times overlap
                if (slot.StartTime <= studentSlot.Slot.EndTime && slot.EndTime >= studentSlot.Slot.StartTime)
                {
                    throw new BadRequestException($"There is a schedule conflict with slot [Start : {studentSlot.Slot.StartTime}; End : {studentSlot.Slot.EndTime}], please check again");
                }
            }
        }
        public async Task<GetSlotsDtos> CreateSlotAsync(CreateSlotsDto slotDto, GetProfileUserDtos user)
        {
            // Add the new Slot entity to repository
            var mappedSlot = slotDto.Adapt<Models.Models.Slot>();
            mappedSlot.CreateById = user.Id;

            await ValidateSlot(mappedSlot);

            var createdSlotEntity = await _unitOfWork.SlotRepository.AddAsync(mappedSlot);
            await _unitOfWork.SaveChangesAsync();

            // Map the created entity back to CreateSlotsDtos and return it
            var createdSlotDto = createdSlotEntity.Entity.Adapt<GetSlotsDtos>();
            return createdSlotDto;
        }

        public async Task<GetSlotsDtos> UpdateSlotAsync(UpdateSlotDto slotDto, GetProfileUserDtos user)
        {
            // Retrieve the existing slot entity from the database
            var existingSlotEntity = await _unitOfWork.SlotRepository.FirstOrDefaultAsync(s => s.Id == slotDto.Id);

            // Check if the entity is null
            if (existingSlotEntity == null)
            {
                throw new NotFoundException($"Slot with ID {slotDto.Id} not found.");
            }

            // Adapt the incoming DTO to the existing entity
            existingSlotEntity = slotDto.Adapt(existingSlotEntity);
            await ValidateSlot(existingSlotEntity);

            // Set the updated fields
            existingSlotEntity.UpdatedById = user.Id; // Assuming there is an UpdatedById property
            existingSlotEntity.UpdatedDate = DateTime.Now; // Assuming there is an UpdatedDate property

            // Update the entity in the database
            var updatedSlotEntity = _unitOfWork.SlotRepository.Update(existingSlotEntity);

            // Save the changes
            await _unitOfWork.SaveChangesAsync();
            // Return the updated DTO
            return updatedSlotEntity.Entity.Adapt<GetSlotsDtos>();
        }


        public async Task<bool> DeleteSlotAsync(int id)
        {
            var isDeleted = await _unitOfWork.SlotRepository.DeleteSlotAsync(id);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.CreateNotificationAsync(new NotificationCreateDto()
            {
                Content = $"Slot với slotid = {id} đã được xóa   ",
                IsViewed = false,
            });
            if (!isDeleted)
            {
                throw new NotFoundException($"Slot with ID {id} not found.");
            }
            return isDeleted;
        }

        public async Task CronJobForAutoDereasedMoneyAfterSlotStart()
        {
            var listSlotDb = await _unitOfWork.SlotRepository.Where(slot => slot.StartTime >= DateTime.Now.AddHours(-1)).ToListAsync();

            foreach (var slot in listSlotDb)
            {
                var slotStudent = await _slotStudentServices.GetSlotStudentById(slot.Id);
                if (slotStudent != null && slotStudent.PaymentStatus == PaymentStatus.Notpaid)
                {
                    var tutor = await _userServices.GetProfileAsync(slot.CreateById, null, null);
                    var duration = (slot.EndTime - slot.StartTime).TotalHours;

                    var studentBalance = await _userServices.GetBalanceAsync(slotStudent.UserId);
                    var amountToDecrease = tutor.TutorFeePerHour * (decimal)duration;
                    decimal slotCost = tutor.TutorFeePerHour * (decimal)duration;
                    if (studentBalance - slotCost >= 0)
                    {
                        await _userServices.UpdateBalanceAsync(slotStudent.UserId, -slotCost);
                        await _transactionServices.CreateTransactionForAutoDecreaMoneySlotAsync(slot.Id, -amountToDecrease);
                        await _slotStudentServices.SlotStudentPaidAsync(slot.Id, slotStudent.UserId);
                    }
                    //else 
                    //{
                    //    await _transactionServices.CreateTransactionForAutoDecreaMoneySlotFailedAsync(slot.Id,
                    //        -amountToDecrease);
                    //}
                }
            }
        }


        // improved listOfSlotIds later, cause it foreach all the slotId in same class if have
        public async Task CronJobForAutoCheckIfStudentDeptIsMoreThan20Percent()
        {
            var notPaidSlotStudents = await _slotStudentServices.GetListSLotStudentByStatus(PaymentStatus.Notpaid);
            var slotIds = notPaidSlotStudents.Select(l => l.SlotId).Distinct().ToList();
            foreach (var slotId in slotIds)
            {
                var slotTotalList = await GetListOfSlotSameClassBySlotId(slotId);
                var totalSlots = slotTotalList.Count;

                if (totalSlots == 0) continue;

                var notPaidSlotsCount = slotTotalList.Count(ls =>
                    ls.SlotStudents.Any(ss => ss.PaymentStatus == PaymentStatus.Notpaid));
                double notPaidSlotsPercentage = (double)notPaidSlotsCount / totalSlots;

                if (notPaidSlotsPercentage >= 0.20)
                {
                    await HandleHighUnpaidSlots(slotId, slotTotalList);
                }
                else if (notPaidSlotsPercentage >= 0.15)
                {
                    await SendPaymentReminder(slotId, slotTotalList);
                }
            }
        }

        private async Task HandleHighUnpaidSlots(int slotId, List<GetSlotWithSlotStudentDto> slotTotalList)
        {
            var slotStudentDto = await _slotStudentServices.GetSlotStudentById(slotId);
            var userDto = await _userServices.GetProfileAsync(slotStudentDto.UserId, null, null);
            var classId = slotTotalList.FirstOrDefault().ClassId;

            if (classId.HasValue)
            {
                var classModel = await _unitOfWork.ClassRepository.FirstOrDefaultAsync(cl => cl.Id == classId);
                var emailParams = new Dictionary<string, string>
                {
                    { "Name", userDto.Email },
                    { "ClassId", classModel.Name ?? classModel.Id.ToString() }
                };

                await SendEmail(EmailType.High_Unpaid_Slots_Warning, userDto.Email, emailParams);

                try
                {
                    await _slotStudentServices.SoftDeleteSlotStudent(slotId, userDto.Id);
                    await _studentClassService.DeleteStudentFromStudentClassById(classModel.Id, userDto.Id);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("StudentClass not found"))
                    {
                        Console.WriteLine($"StudentClass not found for user {userDto.Id} in slot {slotId}. Skipping to next.");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private async Task SendPaymentReminder(int slotId, List<GetSlotWithSlotStudentDto> slotTotalList)
        {
            var slotStudentDto = await _slotStudentServices.GetSlotStudentById(slotId);
            var userDto = await _userServices.GetProfileAsync(slotStudentDto.UserId, null, null);
            var classId = slotTotalList.FirstOrDefault()?.ClassId;

            if (classId.HasValue)
            {
                var classModel = await _unitOfWork.ClassRepository.FirstOrDefaultAsync(cl => cl.Id == classId);
                var emailParams = new Dictionary<string, string>
                {
                    { "Name", userDto.Email },
                    { "ClassId", classModel.Name ?? classModel.Id.ToString() }
                };

                await SendEmail(EmailType.Slot_Payment_Reminder, userDto.Email, emailParams);
            }
        }

        private async Task SendEmail(string emailType, string toAddress, Dictionary<string, string> emailParams)
        {
            var toAddressList = new List<string> { toAddress };
            await _emailServices.SendAsync(emailType, toAddressList, new List<string>(), emailParams);
        }


        public async Task<List<GetSlotWithSlotStudentDto>> GetListOfSlotSameClassBySlotId(int slotId)
        {
            var classId = await _slotRepository.Where(sl => sl.Id == slotId).Select(l => l.ClassId).FirstOrDefaultAsync();
            var listSlotWithSameClass = await _slotRepository.Where(sl => sl.ClassId == classId).ToListAsync();
            return listSlotWithSameClass.Adapt<List<GetSlotWithSlotStudentDto>>();
        }

        public async Task UpdateSlotStatusAsync(UpdateSlotStatusDto updateSlotStatusDto)
        {
            var slotInDb = _unitOfWork.SlotRepository.FirstOrDefault(sl => sl.Id == updateSlotStatusDto.Id);
            slotInDb.SlotStatus = updateSlotStatusDto.Status;
            _unitOfWork.SlotRepository.Update(slotInDb);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> EnrollForSlot(int studentId, int slotId)
        {
            // check if student already enroll this slot
            var existStudentSlot = await _slotStudentServices.GetSlotStudentAsync(studentId, slotId);
            if (existStudentSlot != null)
            {
                throw new BadRequestException($"This user: {studentId} has enroll for this slot {slotId}");
            }

            await ValidateSlotForStudent(slotId, studentId);

            await _slotStudentServices.CreateSlotStudentIfNotExists(slotId, studentId);
            return true;
        }

        //public async Task<List<GetSlotWithSlotStudentDto>> GetListSlotOfStudentByStudentId(int studentId)
        //{
        //    return await _slotRepository.GetSlotWithSlotStudentByStudentId(studentId);
        //}

    }
}


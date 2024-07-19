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
        private readonly IEmailServices _emailServices;
        private readonly IStudentClassService _studentClassService;

        public SlotService(IUnitOfWorkRepository unitOfWorkRepository,
            ISlotStudentServices slotStudentServices, ITransactionServices transactionServices, IUserServices userServices,
            IClassServices classServices, IEmailServices emailServices, IStudentClassService studentClassService,
            ISlotRepository slotRepository, IAuthServices authService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWorkRepository;
            _slotRepository = slotRepository;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _slotStudentServices = slotStudentServices;
            _transactionServices = transactionServices;
            _userServices = userServices;
            _emailServices = emailServices;
            _studentClassService = studentClassService;
        }


        public async Task<PagedResult<GetSlotsDtos>> GetSlotsAsync(PagingModel<QuerySlotDto> request)
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

        public async Task<GetSlotsDtos> CreateSlotAsync(CreateSlotsDto slotDto)
        {
            var slotEntity = slotDto.Adapt<CreateSlotsDto>(); // Assuming Mapster is used for mapping
            
            // Add the new Slot entity to repository
            var createdSlotEntity = await _unitOfWork.SlotRepository.CreateSlotAsync(slotEntity);
            await _unitOfWork.SaveChangesAsync();

            // Map the created entity back to CreateSlotsDtos and return it
            var createdSlotDto = createdSlotEntity.Adapt<GetSlotsDtos>(); // Mapster mapping

            return createdSlotDto;
        }
        public async Task<UpdateSlotDto> UpdateSlotAsync(UpdateSlotDto slotDto)
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
            return updatedSlotEntity.Entity.Adapt<UpdateSlotDto>();
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
            var listSlotDb = await _unitOfWork.SlotRepository.Where(slot => slot.StartTime >= DateTime.Now.AddHours(-1)).ToListAsync();

            foreach (var slot in listSlotDb)
            {
                var slotStudent = await _slotStudentServices.GetSlotStudentById(slot.Id);
                if (slotStudent.PaymentStatus == PaymentStatus.Notpaid && slotStudent != null)
                {
                    var tutor = await _userServices.GetProfileAsync(slot.CreateById, null, null);
                    var duration = (slot.EndTime - slot.StartTime).TotalHours;

                    var studentBalance = await _userServices.GetBalanceAsync(slotStudent.UserId);
                    var amountToDecrease = tutor.TutorFeePerHour * (decimal)duration;
                    decimal slotCost = tutor.TutorFeePerHour * (decimal)duration;
                    if (studentBalance - slotCost >= 0)
                    {
                        await _userServices.UpdateBalanceAsync(slotStudent.UserId, 0, slotCost);
                        await _transactionServices.CreateTransactionForAutoDecreaMoneySlotAsync(slot.Id, -amountToDecrease);
                        await _slotStudentServices.SlotStudentPaidAsync(slot.Id, slotStudent.UserId);
                    }
                    else
                    {
                        await _transactionServices.CreateTransactionForAutoDecreaMoneySlotFailedAsync(slot.Id,
                            -amountToDecrease);
                    }
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
            var classId = slotTotalList.FirstOrDefault()?.ClassId;

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
            var currEnrollSlot = await _slotRepository.FirstOrDefaultAsync(sl => sl.Id == slotId);
            if (currEnrollSlot == null)
            {
                throw new ModelException($"{slotId}", "Slot not found");
            }
            
            // check if student already enroll this slot
            var existEnrollSlot = await _slotRepository.Where(sl =>
                sl.Id == slotId && sl.SlotStudents.Any(ss => ss.UserId == studentId)).FirstOrDefaultAsync();
            if (existEnrollSlot != null)
            {
                throw new ModelException($"{slotId}", $"This user: {studentId} has enroll for this slot {slotId}");   
            }

            // Check if the student is already enrolled in this slot
            var existingEnrollment = await _slotStudentServices.GetSlotStudentById(slotId);
            if (existingEnrollment != null && existingEnrollment.UserId == studentId)
            {
                throw new ModelException($"{slotId}", "Student is already enrolled in this slot");
            }
            var listOfStudentSlots = await GetListSlotOfStudentByStudentId(studentId);
            
            TimeSpan buffer = TimeSpan.FromMinutes(5);
            DateTime adjustedStartTime = currEnrollSlot.StartTime - buffer;
            DateTime adjustedEndTime = currEnrollSlot.EndTime + buffer;

            foreach (var studentSlot in listOfStudentSlots)
            {
                // Exclude the current slot being checked
                if (studentSlot.Id == slotId) continue;

                // Check if the slot times overlap
                if (studentSlot.StartTime < adjustedEndTime && studentSlot.EndTime > adjustedStartTime)
                {
                    throw new ModelException($"{slotId}", $"Conflicts with another slot id: {studentSlot.Id}");
                }
            }
            
            await _slotStudentServices.CreateSlotStudent(slotId, studentId);
            return true;
        }

        public async Task<SlotConflictDto> IsSlotConflict(int slotId, int studentId)
        {
            var currEnrollSlot = await _slotRepository.FirstOrDefaultAsync(sl => sl.Id == slotId);
            if (currEnrollSlot == null)
            {
                throw new ModelException($"{slotId}", "Slot not found");
            }

            var listOfStudentSlots = await GetListSlotOfStudentByStudentId(studentId);
            var conflictDto = new SlotConflictDto();

            if (listOfStudentSlots != null)
            {
                foreach (var slot in listOfStudentSlots)
                {
                    if (slot.Id != slotId && slot.StartTime == currEnrollSlot.StartTime && slot.EndTime == currEnrollSlot.EndTime)
                    {
                        conflictDto.IsConflict = true;
                        conflictDto.conflictSlotId = slot.Id;
                        break; 
                    }
                }
            }

            return conflictDto;
        }


        public async Task<List<GetSlotWithSlotStudentDto>?> GetListSlotOfStudentByStudentId(int studentId)
        {
            return await _slotRepository.GetSlotWithSlotStudentByStudentId(studentId);
        }
    }
}


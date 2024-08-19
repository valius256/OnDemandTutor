using Mapster;
using Microsoft.EntityFrameworkCore;
using MimeKit.Tnef;
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
using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;
using System.Globalization;

namespace OnDemandTutor.BusinessLogic.Services.Slot
{
    public class SlotService : ISlotServices
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly ISlotRepository _slotRepository;
        private readonly IAuthServices _authService;
        private readonly ISlotStudentServices _slotStudentServices;
        private readonly IUserServices _userServices;
        private readonly INotificationService _notificationService;
        private readonly ITransactionServices _transactionServices;

        public SlotService(IUnitOfWorkRepository unitOfWorkRepository,
            ISlotStudentServices slotStudentServices, IUserServices userServices,
            INotificationService notificationService, ITransactionServices transactionServices,
            ISlotRepository slotRepository, IAuthServices authService)
        {
            _unitOfWork = unitOfWorkRepository;
            _slotRepository = slotRepository;
            _authService = authService;
            _slotStudentServices = slotStudentServices;
            _userServices = userServices;
            _notificationService = notificationService;
            _transactionServices = transactionServices;
        }


        public async Task<PagedResult<GetSlotsDtos>> GetSlotsAsync(PagingModel<QuerySlotDto> request)
        {
            return await _unitOfWork.SlotRepository.GetSlotsAsync(request);
        }
        public async Task<GetSlotsDtos> GetClosestSlotOfTutor(GetProfileUserDto tutor)
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
                throw new DataNotFoundException("Slot not found");
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
            var tutorAllSlot = await _slotRepository.Where(sl => sl.CreateById == slot.CreateById && sl.RecordStatus != RecordStatus.Deleted).ToListAsync();
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
                throw new DataNotFoundException("Slot not found");
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
        public async Task<GetSlotsDtos> CreateSlotAsync(CreateSlotsDto slotDto, GetProfileUserDto user)
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
        public async Task<List<Models.Models.Slot>> CreateClassSlotAsync(List<CreateClassSlotDto> slotDtos, GetClassDtos classDto, int userId)
        {
            var results = new List<Models.Models.Slot>();
            foreach (var slotDto in slotDtos)
            {
                var mappedSlot = slotDto.Adapt<Models.Models.Slot>();
                mappedSlot.CreateById = userId;
                mappedSlot.ClassId = classDto.Id;
                mappedSlot.SubjectId = classDto.SubjectId;
                mappedSlot.TeachAddress = classDto.Location;
                mappedSlot.IsOnline = classDto.Method == "Online" ? true : false;
                mappedSlot.ClassId = classDto.Id;
                mappedSlot.NumberOfStudents = classDto.NumberOfStudents;

                await ValidateSlot(mappedSlot);

                var slot = await _unitOfWork.SlotRepository.AddAsync(mappedSlot);
                await _unitOfWork.SaveChangesAsync();
                results.Add(slot.Entity);
            }
            return results;
        }
        public async Task<GetSlotsDtos> UpdateSlotAsync(UpdateSlotDto slotDto, GetProfileUserDto user)
        {
            // Retrieve the existing slot entity from the database
            var existingSlotEntity = await _unitOfWork.SlotRepository.FirstOrDefaultAsync(s => s.Id == slotDto.Id);

            // Check if the entity is null
            if (existingSlotEntity == null)
            {
                throw new DataNotFoundException($"Slot with ID {slotDto.Id} not found.");
            }
            if (user.Id != existingSlotEntity.CreateById)
            {
                throw new ForbiddenException("You have no permission to edit this slot");
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

            //Send noti to students
            var studentsOfSlot = await _slotStudentServices.GetSlotStudentsOfSlotAsync(slotDto.Id);
            await _notificationService.CreateNotificationAsync(new CreateNotificationDto
            {
                Content = $"Slot bạn đã đăng ký học đã có sự thay đổi, vui lòng kiểm tra",
                ReceiverIds = studentsOfSlot.Select(ss => ss.User.Id).ToList(),
                RefImageUrl = user.AvatarImageUrl,
                RefUrl = "/student/schedule"
            });

            // Return the updated DTO
            return updatedSlotEntity.Entity.Adapt<GetSlotsDtos>();
        }

        public async Task UpdateSlotsOfClass(Models.Models.Class classModel)
        {
            foreach (var slot in classModel.Slots)
            {
                if (slot.SlotStatus == SlotStatus.NotYet)
                {
                    slot.SubjectId = classModel.SubjectId;
                    slot.IsOnline = classModel.Method == "Online" ? true : false;
                    slot.TeachAddress = classModel.Location;
                    _unitOfWork.SlotRepository.Update(slot);
                }
            }
            await _unitOfWork.SaveChangesAsync();

        }
        public async Task DeleteSlotAsync(int id)
        {
            var slotDetail = await GetSlotByIdAsync(id);
            if (slotDetail.SlotStatus != SlotStatus.Cancelled)
            {
                throw new BadRequestException("Slot must be cancelled in order to delete pernamently");
            }
            foreach (var slot in slotDetail.SlotStudents)
            {
                if (slotDetail.ClassId == null)
                {
                    await _notificationService.CreateNotificationAsync(new CreateNotificationDto
                    {
                        Content = $"Buổi học môn {slotDetail.Subject.Name} lúc {slotDetail.StartTime} đã bị gia sư xóa. Bạn sẽ được hoàn lại {slot.PaidValue.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"))}",
                        RefImageUrl = slotDetail.CreatedBy.AvatarImageUrl,
                        RefUrl = "/student/schedule",
                        ReceiverIds = new List<int> { slot.UserId }
                    });
                }

                if (slot.PaymentStatus == PaymentStatus.Paid)
                {
                    await _slotStudentServices.Refund(slot.SlotId, slot.UserId);
                }
                await _slotStudentServices.SoftDeleteSlotStudent(slot.SlotId, slot.UserId);
            }
            var existedSlot = await _unitOfWork.SlotRepository.FirstOrDefaultAsync(s => s.Id == id);
            existedSlot.RecordStatus = RecordStatus.Deleted;
            existedSlot.DeletedDate = DateTime.Now;
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<List<GetSlotWithSlotStudentDto>> GetListOfSlotSameClassBySlotId(int slotId)
        {
            var classId = await _slotRepository.Where(sl => sl.Id == slotId).Select(l => l.ClassId).FirstOrDefaultAsync();
            var listSlotWithSameClass = await _slotRepository.Where(sl => sl.ClassId == classId).ToListAsync();
            return listSlotWithSameClass.Adapt<List<GetSlotWithSlotStudentDto>>();
        }
        public async Task ToggleSlotCancellation(int slotId, GetProfileUserDto user)
        {
            var slotInDb = await _unitOfWork.SlotRepository.FirstOrDefaultAsync(sl => sl.Id == slotId);
            if (slotInDb == null)
            {
                throw new DataNotFoundException("Slot not found");
            }
            if (slotInDb.CreateById != user.Id)
            {
                throw new ForbiddenException("This slot does not belong to this tutor");
            }
            if ((slotInDb.SlotStatus != SlotStatus.Cancelled && slotInDb.SlotStatus != SlotStatus.NotYet) || slotInDb.StartTime <= DateTime.Now)
            {
                throw new BadRequestException("This slot is happened, so it is no longer can be changed");
            }
            SlotStatus newStatus = slotInDb.SlotStatus == SlotStatus.Cancelled ? SlotStatus.NotYet : SlotStatus.Cancelled;
            await UpdateSlotStatusAsync(new UpdateSlotStatusDto { Id = slotId, Status = newStatus });
        }

        public async Task UpdateSlotStatusAsync(UpdateSlotStatusDto updateSlotStatusDto)
        {
            var slotInDb = _unitOfWork.SlotRepository.FirstOrDefault(sl => sl.Id == updateSlotStatusDto.Id);
            slotInDb.SlotStatus = updateSlotStatusDto.Status;
            _unitOfWork.SlotRepository.Update(slotInDb);
            await _unitOfWork.SaveChangesAsync();

            //Notification
            var slotDetail = await GetSlotByIdAsync(updateSlotStatusDto.Id);
            var message = "";
            if (updateSlotStatusDto.Status == SlotStatus.Cancelled)
            {
                message = $"Buổi học môn {slotDetail.Subject.Name} lúc {slotDetail.StartTime} đã bị hủy, bạn có thể thoát khỏi buổi học này để được hoàn lại tiền.";
            } else if (updateSlotStatusDto.Status == SlotStatus.NotYet)
            {
                message = $"Buổi học môn {slotDetail.Subject.Name} lúc {slotDetail.StartTime} đã được mở lại";
            }
            if (message != "" && slotDetail.ClassId == null)
            {
                await _notificationService.CreateNotificationAsync(new CreateNotificationDto
                {
                    Content = message,
                    RefImageUrl = slotDetail.CreatedBy.AvatarImageUrl,
                    RefUrl = "/student/schedule",
                    ReceiverIds = slotDetail.SlotStudents.Select(ss => ss.UserId).ToList()
                });
            }      
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

            //Notification
            var student = await _userServices.GetProfileAsync(studentId, null, null);
            var slot = await GetSlotByIdAsync(slotId);
            await _notificationService.CreateNotificationAsync(new CreateNotificationDto
            {
                Content = $"1 học viên đã đăng ký slot {slot.StartTime} đến {slot.EndTime} của bạn.",
                ReceiverIds = new List<int> { slot.CreateById },
                RefImageUrl = student.AvatarImageUrl,
                RefUrl = "/tutor/schedule"
            });

            return true;
        }

        public async Task CronJobForTransferringMoneyToTutor()
        {
            var slots = await _unitOfWork.SlotRepository.GetFinishedSlotsToTransfer();
            foreach ( var slot in slots )
            {
                await Transfer(slot.Id);
            }
        }
        private async Task Transfer(int slotId)
        {
            var slot = await GetSlotByIdAsync(slotId);
            var slotStudents = await _slotStudentServices.GetSlotStudentsOfSlotAsync(slotId);
            var isFull = true;
            decimal totalAmount = 0;
            List<TransactionDto> transactions = new List<TransactionDto>();
            foreach (var slotStudent in slotStudents)
            {
                if (slotStudent.PaymentStatus == PaymentStatus.Paid)
                {
                    await _userServices.UpdateBalanceAsync(slot.CreateById, slotStudent.PaidValue);
                    transactions.Add(new TransactionDto
                    {
                        TransactionCode = $"HP_Slot_{slot.Id}_Tutor_{slot.CreateById}_{DateTime.Now.Ticks}",
                        Amount = slotStudent.PaidValue,
                        CreatedById = slot.CreateById,
                        CreatedDate = DateTime.Now,
                        Notes = $"Thanh toán slot {slot.Subject.Name} lúc {slot.StartTime} từ học sinh {slotStudent.User.FirstName ?? ""} {slotStudent.User.LastName ?? ""}",
                        PaymentMethod = "Internal",
                        Status = PaymentStatus.Paid,
                        TransactionType = TransactionType.Receive_money
                    });
                    await _slotStudentServices.SetTransferred(slotStudent.Id);
                    totalAmount += slotStudent.PaidValue;
                } else if (isFull)
                {
                    isFull = false;
                }
            }
            await _notificationService.CreateNotificationAsync(new CreateNotificationDto
            {
                Content = $"Bạn đã nhận được {totalAmount.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"))} từ buổi học {slot.Subject.Name} lúc {slot.StartTime}. " +
                $"{(isFull ? "Mọi người đã thanh toán đầy đủ" : "Tuy nhiên vẫn còn 1 số học viên chưa thanh toán, bạn có thể nhắc nhở họ")}",
                RefUrl = "/tutor/schedule",
                ReceiverIds = new List<int> { slot.CreateById },
                RefImageUrl = "/src/assets/logo.png"
            });
            await _transactionServices.CreateTransactionDb(transactions);
        }
    }
}


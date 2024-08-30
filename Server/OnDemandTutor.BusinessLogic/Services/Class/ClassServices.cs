using Mapster;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.BusinessLogic.Interfaces.Class;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.BusinessLogic.Interfaces.Slot;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;
using System.Globalization;

namespace OnDemandTutor.BusinessLogic.Services.Class
{
    public class ClassServices : IClassServices
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly ISlotStudentServices _slotStudentServices;
        private readonly ISlotServices _slotServices;
        private readonly IUserServices _userServices;
        private readonly ITransactionServices _transactionServices;

        public ClassServices(IUnitOfWorkRepository unitOfWork, INotificationService notificationService, 
            ISlotStudentServices slotStudentServices, ISlotServices slotServices, IUserServices userServices, ITransactionServices transactionServices)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _slotStudentServices = slotStudentServices;
            _slotServices = slotServices;
            _userServices = userServices;
            _transactionServices = transactionServices;
        }

        public async Task<PagedResult<GetClassDtos>> GetClasses(PagingModel<QueryClassDTO> request)
        {
            var pagedResult = await _unitOfWork.ClassRepository.GetClasses(request);
            var mappedResult = pagedResult.Adapt<PagedResult<GetClassDtos>>();
            foreach (var result in mappedResult.Items)
            {
                var class_ = pagedResult.Items.FirstOrDefault(x => x.Id == result.Id);
                var classSlots = class_?.Slots.OrderBy(s => s.StartTime).ToList() ?? new List<Models.Models.Slot>();
                if (classSlots.Any())
                {
                    result.StartTime = classSlots[0].StartTime;
                    result.EndTime = classSlots[classSlots.Count - 1].EndTime;
                }
            }

            return mappedResult;
        }

        public async Task<PagedResult<GetClassDtos>> GetClassesOfStudent(int studentId, int page, int limit)
        {
            var pagedResult = await _unitOfWork.ClassRepository.GetClassesOfStudent(studentId, page, limit);
            var mappedResult = pagedResult.Adapt<PagedResult<GetClassDtos>>();
            foreach (var result in mappedResult.Items)
            {
                var class_ = pagedResult.Items.FirstOrDefault(x => x.Id == result.Id);
                var classSlots = class_?.Slots.OrderBy(s => s.StartTime).ToList() ?? new List<Models.Models.Slot>();
                if (classSlots.Any())
                {
                    result.StartTime = classSlots[0].StartTime;
                    result.EndTime = classSlots[classSlots.Count - 1].EndTime;
                }
            }

            return mappedResult;
        }

        public async Task<PagedResult<GetClassDtos>> GetClassesOfTutor(int studentId, int page, int limit)
        {
            var pagedResult = await _unitOfWork.ClassRepository.GetClassesOfTutor(studentId, page, limit);
            var mappedResult = pagedResult.Adapt<PagedResult<GetClassDtos>>();
            foreach (var result in mappedResult.Items)
            {
                var class_ = pagedResult.Items.FirstOrDefault(x => x.Id == result.Id);
                var classSlots = class_?.Slots.OrderBy(s => s.StartTime).ToList() ?? new List<Models.Models.Slot>();
                if (classSlots.Any())
                {
                    result.StartTime = classSlots[0].StartTime;
                    result.EndTime = classSlots[classSlots.Count - 1].EndTime;
                }
            }

            return mappedResult;
        }

        public async Task<GetClassFullDataSlotDto> GetClassByIdAsync(int id)
        {
            var classEntity = await _unitOfWork.ClassRepository.GetClassWithSlotsByIdAsync(id);

            if (classEntity is null)
            {
                throw new Exception("Class not found");
            }

            var rs = classEntity.Adapt<GetClassFullDataSlotDto>();
            var classSlots = classEntity?.Slots.OrderBy(s => s.StartTime).ToList() ?? new List<Models.Models.Slot>();
            if (classSlots.Any())
            {
                rs.StartTime = classSlots[0].StartTime;
                rs.EndTime = classSlots[classSlots.Count - 1].EndTime;
            }

            return rs;
        }

        public async Task<GetClassDtos> CreateClassAsync(CreateClassDTO classDto, GetProfileUserDto user)
        {
            var classEntity = classDto.Adapt<Models.Models.Class>();
            classEntity.TutorId = user.Id;
            classEntity.Status = ClassStatus.NotStart;
            var createdClass = await _unitOfWork.ClassRepository.AddAsync(classEntity);
            await _unitOfWork.SaveChangesAsync();
            var rs = createdClass.Entity.Adapt<GetClassDtos>();
            await _slotServices.CreateClassSlotAsync(classDto.SlotList, rs, user.Id);
            return rs;
        }

        public async Task<GetClassDtos> UpdateClassAsync(UpdateClassDto classDto, GetProfileUserDto user)
        {
            var existClass = await _unitOfWork.ClassRepository.FirstOrDefaultAsync(c => c.Id == classDto.Id);
            if (existClass == null) 
            {
                throw new DataNotFoundException("Class not found");
            }
            if (user.Id != existClass.TutorId)
            {
                throw new ForbiddenException("You have no permission to edit this class");
            }
            if (existClass.Status == ClassStatus.Finished)
            {
                throw new BadRequestException("Class is unable to edit");
            }
            var classDetail = await GetClassByIdAsync(existClass.Id);
            var listOfStudentInClass = classDetail.StudentClasses;
            if (classDto.NumberOfStudents < listOfStudentInClass.Count)
            {
                throw new BadRequestException("Cannot update number of students which is smaller than the current number of students in the class");
            }

            // Map the changes to the existing class entity
            classDto.Adapt(existClass);
            _unitOfWork.ClassRepository.Update(existClass);
            await _unitOfWork.SaveChangesAsync();

            //Update slots
            classDetail = await GetClassByIdAsync(existClass.Id); //Re-fetch
            await _slotServices.UpdateSlotsOfClass(classDetail.Adapt<Models.Models.Class>());
            var createdSlots = await _slotServices.CreateClassSlotAsync(classDto.NewClassSlots, classDetail.Adapt<GetClassDtos>(), classDetail.TutorId);
            foreach (var studentClass in listOfStudentInClass)
            {
                foreach(var slot in createdSlots)
                {
                    await _slotStudentServices.CreateSlotStudentIfNotExists(slot.Id , studentClass.StudentId);
                }
            }

            await _notificationService.CreateNotificationAsync(new CreateNotificationDto()
            {
                Content = $"Lớp {classDetail.Name} của bạn có sự thay đổi, vui lòng kiểm tra lại",
                ReceiverIds = listOfStudentInClass.Select(sc => sc.StudentId).ToList(),
                RefImageUrl = classDetail.Tutor.AvatarImageUrl,
                RefUrl = "/student/myclass"
            }) ;
            return classDetail.Adapt<GetClassDtos>();
        }

        public async Task<bool> DeleteClassAsync(int id)
        {
            var classEntity = await _unitOfWork.ClassRepository.FirstOrDefaultAsync(c => c.Id == id);
            if (classEntity == null)
            {
                throw new Exception("Class not found");
            }
            classEntity.SoftDelete();
            _unitOfWork.ClassRepository.Update(classEntity);
            await _unitOfWork.SaveChangesAsync();

            //Delete Slots
            var classDetail = await GetClassByIdAsync(id);
            foreach (var slot in classDetail.Slots)
            {
                await _slotServices.DeleteSlotAsync(slot.Id);
            }
            //Refund deposit
            foreach (var studentClass in classDetail.StudentClasses)
            {
                if (studentClass.DepositPaid != null)
                {
                    await _userServices.UpdateBalanceAsync(studentClass.StudentId, studentClass.DepositPaid.Value);
                     await _transactionServices.CreateTransactionDb(new List<Models.Dtos.Transaction.GetTransactionDto> { new Models.Dtos.Transaction.GetTransactionDto
                    {
                        ClassId = classDetail.Id,
                        CreatedById = studentClass.StudentId,
                        CreatedDate = DateTime.Now,
                        TransactionCode = "RefundDeposit_" + DateTime.Now.Ticks,
                        Notes = "Hoàn trả tiền cọc lớp " + classDetail.Name,
                        Amount = studentClass.DepositPaid.Value,
                        PaymentMethod = "Internal",
                        Status = PaymentStatus.Paid,
                        TransactionType = TransactionType.Receive_money,
                    } });
                }       
            }      
            //Sending notification
            await _notificationService.CreateNotificationAsync(new CreateNotificationDto
            {
                Content = $"Lớp học {classDetail.Name} đã bị gia sư xóa vĩnh viễn. Bạn sẽ được hoàn lại tiền cọc",
                RefImageUrl = classDetail.Tutor.AvatarImageUrl,
                RefUrl = "/student/myclass",
                ReceiverIds = classDetail.StudentClasses.Select(sc => sc.StudentId).ToList()
            });
            return true;
        }

        public async Task CronForAutoChangeStatusClassAndSlot()
        {
            var slots = await _slotServices.GetSlotsAsync(new PagingModel<QuerySlotDto>()
            {
                Filter = new QuerySlotDto()
                {
                    IsAboutToEnd = true,
                    SlotStatus = new List<SlotStatus> { SlotStatus.OnGoing, SlotStatus.NotYet }
                },
                Page = 1,
                Limit = int.MaxValue
            });

            foreach (var slot in slots.Items)
            {
                SlotStatus newStatus;
                if (slot.SlotStatus == SlotStatus.OnGoing)
                {
                    newStatus = SlotStatus.Finished;
                }
                else
                {
                    newStatus = SlotStatus.OnGoing;
                }
                await _slotServices.UpdateSlotStatusAsync(new UpdateSlotStatusDto() { Id = slot.Id, Status = newStatus });
                if (slot.ClassId != null)
                {
                    await UpdateStatusOfClassDueToSlotChange(slot.ClassId.Value);
                }
            }
        }

        private async Task UpdateStatusOfClassDueToSlotChange(int classId)
        {
            var classDetail = await GetClassByIdAsync(classId);
            //Avoid update navigators
            var classModel = await _unitOfWork.ClassRepository.FindAsync(classId);
            if (classModel == null)
            {
                throw new DataNotFoundException("Class not found");
            }
            if (classDetail.Slots.All(s => s.SlotStatus == SlotStatus.Finished))
            {
                classModel.Status = ClassStatus.Finished;
            }
            if (classDetail.Slots.Any(s => s.SlotStatus == SlotStatus.OnGoing))
            {
                classModel.Status = ClassStatus.OnGoing;
            }
            _unitOfWork.ClassRepository.Update(classModel);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ValidateClassForStudent(int classId, int studentId)
        {
            var listOfStudentSlots = await _slotStudentServices.GetSimpleStudentSlotOfStudent(studentId);
            var classDetail = await _unitOfWork.ClassRepository.GetClassWithSlotsByIdAsync(classId);
            if (classDetail == null)
            {
                throw new DataNotFoundException("Class not found");
            }
            foreach (var classSlot in classDetail.Slots)
            {
                foreach (var studentSlot in listOfStudentSlots)
                {
                    // Check if the slot times overlap
                    if (classSlot.StartTime <= studentSlot.Slot.EndTime && classSlot.EndTime >= studentSlot.Slot.StartTime)
                    {
                        throw new BadRequestException($"Slot [Start : {classSlot.StartTime}; End : {classSlot.EndTime}] of the class has conflict with a current slot of student" +
                            $" [Start : {studentSlot.Slot.StartTime}; End : {studentSlot.Slot.EndTime}], please check again");
                    }
                }
            }

        }

        public async Task ToggleClassCancellation(int classId, GetProfileUserDto user)
        {
            var classData = await _unitOfWork.ClassRepository.FirstOrDefaultAsync(c => c.Id == classId)
                             ?? throw new DataNotFoundException("Class not found");

            if (classData.Status == ClassStatus.Finished)
            {
                throw new BadRequestException("Class must be not finished to use this feature");
            }

            var classDetail = await GetClassByIdAsync(classId);
            if (classDetail.Slots.FirstOrDefault()?.StartTime < DateTime.Now && classData.Status == ClassStatus.Disabled)
            {
                throw new BadRequestException("Class is no longer changeable");
            }

            classData.Status = classData.Status == ClassStatus.Disabled ? ClassStatus.NotStart : ClassStatus.Disabled;
            _unitOfWork.ClassRepository.Update(classData);
            await _unitOfWork.SaveChangesAsync();

            var slotStatus = classData.Status == ClassStatus.Disabled ? SlotStatus.Cancelled : SlotStatus.NotYet;
            var notificationContent = classData.Status == ClassStatus.Disabled
                ? $"Lớp {classDetail.Name} đã bị vô hiệu hóa. Bạn có thể rời khỏi lớp này để được hoàn lại tiền cọc"
                : $"Lớp {classDetail.Name} đã được mở lại và có thể hoạt động như bình thường";

            foreach (var slot in classDetail.Slots)
            {
                await _slotServices.UpdateSlotStatusAsync(new UpdateSlotStatusDto { Id = slot.Id, Status = slotStatus });
            }

            await _notificationService.CreateNotificationAsync(new CreateNotificationDto()
            {
                Content = notificationContent,
                ReceiverIds = classDetail.StudentClasses.Select(sc => sc.StudentId).ToList(),
                RefUrl = "/student/myclass",
                RefImageUrl = user.AvatarImageUrl
            });
        }

    }

}

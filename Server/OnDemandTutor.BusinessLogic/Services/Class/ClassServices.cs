using Mapster;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.BusinessLogic.Interfaces.Class;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.BusinessLogic.Interfaces.Slot;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.Class
{
    public class ClassServices : IClassServices
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly ISlotStudentServices _slotStudentServices;

        public ClassServices(IUnitOfWorkRepository unitOfWork, INotificationService notificationService, ISlotStudentServices slotStudentServices)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _slotStudentServices = slotStudentServices;
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

        public async Task<GetClassDtos> CreateClassAsync(CreateClassDTO classDto, GetProfileUserDtos user)
        {
            var classEntity = classDto.Adapt<Models.Models.Class>();
            classEntity.TutorId = user.Id;

            var createdClass = await _unitOfWork.ClassRepository.AddAsync(classEntity);
            var rs = createdClass.Entity.Adapt<GetClassDtos>();
            await _unitOfWork.SaveChangesAsync();
            return rs;
        }

        public async Task<GetClassDtos> UpdateClassAsync(GetClassDtos classDto)
        {
            var classEntity = classDto.Adapt<Models.Models.Class>();
            var updatedClass = _unitOfWork.ClassRepository.Update(classEntity);

            var receiverIds = new List<int>();
            var listOfStudentInClass = await GetAllStudentInClassWithClassId(updatedClass.Entity.Id);
            var listOfStudentInClassId = listOfStudentInClass.Select(ld => ld.StudentId).ToList();

            receiverIds.Add(classDto.TutorId);
            receiverIds.AddRange(listOfStudentInClassId);
            //await _notificationService.CreateNotificationAsync(new CreateNotificationDto()
            //{
            //    Content = $"this class {classDto.Id} has been update",
            //    IsViewed = false,
            //    ReceiverId = receiverIds
            //});
            await _unitOfWork.SaveChangesAsync();
            return updatedClass.Entity.Adapt<GetClassDtos>();
        }

        public async Task<bool> DeleteClassAsync(int id)
        {
            var classEntity = await _unitOfWork.ClassRepository.FirstOrDefaultAsync(c => c.Id == id);
            if (classEntity == null)
            {
                throw new Exception("Class not found");
            }

            _unitOfWork.ClassRepository.Remove(classEntity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task CronForAutoChangeStatusClassAndSlot()
        {
            var classesToUpdate = await _unitOfWork.ClassRepository
                .WhereAsync(cl => cl.Status == ClassStatus.NotStart || cl.Status == ClassStatus.OnGoing);
            var slotsToUpdate = await _unitOfWork.SlotRepository
                .WhereAsync(sl => sl.SlotStatus == SlotStatus.NotYet || sl.SlotStatus == SlotStatus.OnGoing);

            foreach (var slot in slotsToUpdate)
            {
                if (slot.StartTime <= DateTime.Now && slot.SlotStatus == SlotStatus.NotYet)
                {
                    slot.SlotStatus = SlotStatus.OnGoing;
                }

                if (slot.EndTime <= DateTime.Now && slot.SlotStatus == SlotStatus.OnGoing)
                {
                    slot.SlotStatus = SlotStatus.Finished;
                }
            }

            // Update slots in bulk
            _unitOfWork.SlotRepository.UpdateRange(slotsToUpdate);

            foreach (var classModel in classesToUpdate)
            {
                bool allSlotsFinished = true;

                foreach (var slot in classModel.Slots.ToList())
                {
                    if (slot.StartTime <= DateTime.Now && slot.SlotStatus == SlotStatus.NotYet)
                    {
                        slot.SlotStatus = SlotStatus.OnGoing;
                    }

                    if (slot.EndTime <= DateTime.Now && slot.SlotStatus == SlotStatus.OnGoing)
                    {
                        slot.SlotStatus = SlotStatus.Finished;
                    }

                    if (slot.SlotStatus != SlotStatus.Finished)
                    {
                        allSlotsFinished = false;
                    }
                }

                // Update class status
                if (classModel.Status == ClassStatus.NotStart &&
                    classModel.Slots.Any(sl => sl.SlotStatus == SlotStatus.OnGoing))
                {
                    classModel.Status = ClassStatus.OnGoing;
                }

                if (allSlotsFinished && classModel.Status == ClassStatus.OnGoing)
                {
                    classModel.Status = ClassStatus.Finished;
                }
            }

            // Update classes in bulk
            _unitOfWork.ClassRepository.UpdateRange(classesToUpdate);

            // Save all changes in one go
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> EnrollCLass(int classId, int studentId)
        {
            await ValidateClassForStudent(classId, studentId);
            var classToEnroll = await _unitOfWork.ClassRepository.GetClassWithSlotsByIdAsync(classId);

            // Create a new StudentClass entity
            var studentClass = new Models.Models.StudentClass()
            {
                ClassId = classId,
                StudentId = studentId,
            };

            var receiverId = new List<int>
            {
                studentId,
                classToEnroll!.TutorId
            };

            //await _notificationService.CreateNotificationAsync(new CreateNotificationDto()
            //{
            //    Content = $"User {studentId} đã tham gia class: {classToEnroll!.Name} thành công",
            //    IsViewed = false,
            //    ReceiverId = receiverId,
            //});
            // Add the student to the class
            classToEnroll.StudentClasses.Add(studentClass);
            _unitOfWork.ClassRepository.Update(classToEnroll);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<List<Models.Models.StudentClass>> GetAllStudentInClassWithClassId(int classId)
        {
            return await _unitOfWork.StudentClassRepository.Where(sc => sc.ClassId == classId).ToListAsync();
        }

        public async Task ValidateClassForStudent(int classId, int studentId)
        {
            var listOfStudentSlots = await _slotStudentServices.GetSimpleStudentSlotOfStudent(studentId);
            var classDetail = await _unitOfWork.ClassRepository.GetClassWithSlotsByIdAsync(classId);
            if (classDetail == null)
            {
                throw new NotFoundException("Class not found");
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
    }

}

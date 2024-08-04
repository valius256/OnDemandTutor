using Mapster;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Class;
using OnDemandTutor.BusinessLogic.Interfaces.Mail;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.BusinessLogic.Interfaces.Slot;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Interfaces.StudentClass;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.BusinessLogic.Services.Slot;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.StudentClass;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;
using static OnDemandTutor.Models.Dtos.Blog.GetBlogDtos;

namespace OnDemandTutor.BusinessLogic.Services.StudentClass
{
    public class StudentClassService : IStudentClassService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IAuthServices _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IClassServices _classServices;
        private readonly IUserServices _userServices;
        private readonly INotificationService _notificationService;
        private readonly ISlotServices _slotServices;
        private readonly ISlotStudentServices _slotStudentServices;
        private readonly IEmailServices _emailServices;

        public StudentClassService(IUnitOfWorkRepository unitOfWork, IAuthServices authService,
            IUserServices userServices, IClassServices classServices, INotificationService notificationService, IHttpContextAccessor HttpContextAccessor,
            ISlotServices slotServices, ISlotStudentServices slotStudentServices, IEmailServices emailServices)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _classServices = classServices;
            _userServices = userServices;
            _httpContextAccessor = HttpContextAccessor;
            _notificationService = notificationService;
            _slotServices = slotServices;
            _slotStudentServices = slotStudentServices;
            _emailServices = emailServices;
        }
        //public async Task<PagedResult<GetStudentClassDto>> GetStudentClassesAsync(PagingModel<GetStudentClassDto> pagingModel)
        //{
        //    var pagedResult = await _unitOfWork.StudentClassRepository.PagingAsync(pagingModel.Adapt<PagingModel<Models.Models.StudentClass>>());
        //    return pagedResult.Adapt<PagedResult<GetStudentClassDto>>();
        //}
        public async Task<PagedResult<GetStudentClassDetailDto>> QueryStudentClassAsync(PagingModel<QueryStudentClassDto> querySlotStudentDto)
        {
            var slotStudent =
                await _unitOfWork.StudentClassRepository.QueryStudentClass(querySlotStudentDto);
            return slotStudent.Adapt<PagedResult<GetStudentClassDetailDto>>();
        }
        public async Task<GetStudentClassDto> GetStudentClassByIdAsync(int id)
        {
            var studentClass = await _unitOfWork.StudentClassRepository.FirstOrDefaultAsync(sc => sc.Id == id);
            return studentClass.Adapt<GetStudentClassDto>();
        }

        public async Task<CreateStudentClassDto> CreateStudentClassAsync(CreateStudentClassDto studentClassDto)
        {
            var studentClass = studentClassDto.Adapt<Models.Models.StudentClass>();
            var createdStudentClass = await _unitOfWork.StudentClassRepository.AddAsync(studentClass);
            // get name of student 
            var student =
                await _unitOfWork.UserRepository.FirstOrDefaultAsync(ld => ld.Id == studentClassDto.StudentId);
            await _unitOfWork.SaveChangesAsync();
            var classEntity =
                await _unitOfWork.ClassRepository.FirstOrDefaultAsync(ld => ld.Id == studentClassDto.ClassId);
           
            //await _notificationService.CreateNotificationAsync(new CreateNotificationDto()
            //{
            //    Content = $"Bạn {student.FirstName} {student.LastName} đã được thêm vào lớp {studentClass.Class.Name} thành công ",
            //    IsViewed = true,
            //    ReceiverId = new List<int> { studentClass.StudentId, classEntity.TutorId }
            //});
            return createdStudentClass.Entity.Adapt<CreateStudentClassDto>();
        }
        public async Task<UpdateStudentClassDto> UpdateStudentClassAsync(UpdateStudentClassDto studentClassDto)
        {
            // Retrieve the existing student class entity from the database
            var existingStudentClassEntity = await _unitOfWork.StudentClassRepository.FirstOrDefaultAsync(sc => sc.Id == studentClassDto.Id);

            // Check if the entity is null
            if (existingStudentClassEntity == null)
            {
                throw new NotFoundException($"StudentClass with ID {studentClassDto.Id} not found.");
            }

            // Retrieve the user profile
            var user = await _authService.GetUserProfileByClaim(_httpContextAccessor.HttpContext.User);

            // Adapt the incoming DTO to the existing entity
            existingStudentClassEntity = studentClassDto.Adapt(existingStudentClassEntity);

            // Update the entity fields if needed
            existingStudentClassEntity.UpdatedDate = DateTime.Now; // Assuming you want to update this field

            // Update the entity in the database
            var updatedStudentClassEntity = _unitOfWork.StudentClassRepository.Update(existingStudentClassEntity);

            // Save the changes
            await _unitOfWork.SaveChangesAsync();

            // Return the updated DTO
            return updatedStudentClassEntity.Entity.Adapt<UpdateStudentClassDto>();
        }


        public async Task<bool> DeleteStudentClassAsync(int id)
        {
            var studentClass = await _unitOfWork.StudentClassRepository.FirstOrDefaultAsync(sc => sc.Id == id);
            if (studentClass == null)
            {
                throw new Exception("StudentClass not found");
            }
            _unitOfWork.StudentClassRepository.Remove(studentClass);
            await _unitOfWork.SaveChangesAsync();

            //await _notificationService.CreateNotificationAsync(new CreateNotificationDto()
            //{
            //    Content = $"Bạn đã bị xóa khỏi lớp {studentClass.ClassId}  ",
            //    IsViewed = true,
            //    ReceiverId =new List<int>(studentClass.StudentId),
            //});
            return true;
        }

        public async Task<bool> StudentRatingClassAsync(int classId, int studentId, int Rating, string? Feedback)
        {
            var recordInDB = await _unitOfWork.StudentClassRepository.FirstOrDefaultAsync(st => st.StudentId == studentId && st.ClassId == classId);
            if (recordInDB == null)
            {
                throw new NotFoundException($"StudentClass has not found");
            }

            // handle for rating in student class
            recordInDB.Rating = Rating;
            recordInDB.Feedback = Feedback;
            _unitOfWork.StudentClassRepository.Update(recordInDB);
            await _unitOfWork.SaveChangesAsync();

            // handle for update tutor rating 
            var classModel = await _classServices.GetClassByIdAsync(recordInDB.ClassId);
            var tutorId = classModel.TutorId;
            
            await _userServices.RecalculateTutorRating(tutorId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Models.Models.StudentClass> CreateStudentClassIfNotExist(int classId, int studentId)
        {
            var recordInDb = await _unitOfWork.StudentClassRepository.FirstOrDefaultAsync(st =>
                st.ClassId == classId && st.StudentId == studentId);
            if (recordInDb == null)
            {
                recordInDb = new Models.Models.StudentClass()
                {
                    ClassId = classId,
                    StudentId = studentId,
                };
                var newStudentClassEntity =  await _unitOfWork.StudentClassRepository.AddAsync(recordInDb);
                //await _notificationService.CreateNotificationAsync(new CreateNotificationDto()
                //{
                //    Content = $"Bạn đã được thêm vào class {newStudentClassEntity.Entity.Class.Name}",
                //    IsViewed = true,
                //    ReceiverId =new List<int>(newStudentClassEntity.Entity.StudentId),
                //});
                await _unitOfWork.SaveChangesAsync();
            }
          
            return recordInDb;
        }


        public async Task<bool> DeleteStudentClass(int classId, int userId)
        {
            var studentClass = await _unitOfWork.StudentClassRepository.FirstOrDefaultAsync(sc => sc.ClassId == classId && sc.StudentId == userId);
            if (studentClass == null)
            {
                throw new Exception("StudentClass not found");
            }
            _unitOfWork.StudentClassRepository.Remove(studentClass);
            await _unitOfWork.SaveChangesAsync();
            //await _notificationService.CreateNotificationAsync(new CreateNotificationDto()
            //{
            //    Content = $"Bạn đã bị xóa khỏi class {studentClass.Class.Name}",
            //    IsViewed = true,
            //    ReceiverId = new List<int>(studentClass.StudentId),
            //});
            return true;
        }

        public async Task<bool> EnrollClass(int classId, int studentId)
        {
            await _classServices.ValidateClassForStudent(classId, studentId);
            var student = await _userServices.GetProfileAsync(studentId, null, null);
            if (student == null)
            {
                throw new NotFoundException("Student not found");
            }
            var classToEnroll = await _classServices.GetClassByIdAsync(classId);
            if (classToEnroll == null)
            {
                throw new NotFoundException("Class not found");
            }
            // Create a new StudentClass entity
            await _unitOfWork.StudentClassRepository.AddAsync(new Models.Models.StudentClass
            {
                StudentId = studentId,
                ClassId = classId
            });
            
            // Create Student Slots
            foreach (var slot in classToEnroll.Slots)
            {
                await _slotStudentServices.CreateSlotStudentIfNotExists(slot.Id, studentId);
            }

            //Noti tutor and student
            await _notificationService.CreateNotificationAsync(new CreateNotificationDto()
            {
                Content = $"1 học sinh đã tham gia lớp {classToEnroll!.Name} của bạn",
                ReceiverIds = new List<int> { classToEnroll.TutorId },
                RefUrl = "/tutor/myclass/list",
                RefImageUrl = student.AvatarImageUrl
            });
            await _notificationService.CreateNotificationAsync(new CreateNotificationDto()
            {
                Content = $"Bạn đã tham gia lớp {classToEnroll!.Name} thành công. Chúc bạn và gia sư có 1 quá trình học gặt hái được nhiều thành công.",
                ReceiverIds = new List<int>{studentId},
                RefUrl = "/student/myclass",
                RefImageUrl = classToEnroll.Tutor.AvatarImageUrl
            });

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // improved listOfSlotIds later, cause it foreach all the slotId in same class if have
        public async Task CronJobForAutoCheckIfStudentDeptIsMoreThan20Percent()
        {
            var studentClasses = await _unitOfWork.StudentClassRepository.GetAllStudentClassesThatHaveAtLeastOneDebtSlot();
            foreach (var studentClass in studentClasses)
            {
                var totalSlots = studentClass.Class.Slots.Count;
                int deptSlots = 0;
                foreach (var slot in studentClass.Class.Slots)
                {
                    var studentSlot = slot.SlotStudents.FirstOrDefault(s => s.SlotId == slot.Id && s.UserId == studentClass.StudentId);
                    if (studentSlot == null) continue;
                    if (studentSlot.PaymentStatus == PaymentStatus.Notpaid) deptSlots++;
                }

                if (totalSlots == 0) continue;
                double notPaidSlotsPercentage = (double)deptSlots / totalSlots;

                if (notPaidSlotsPercentage >= 0.20)
                {
                    await HandleHighUnpaidSlots(studentClass);
                }
                else if (notPaidSlotsPercentage >= 0.15)
                {
                    await SendPaymentReminder(studentClass);
                }
            }
        }

        private async Task HandleHighUnpaidSlots(Models.Models.StudentClass studentClass)
        {
            var emailParams = new Dictionary<string, string>
            {
                { "Name", studentClass.Student.Email },
                { "ClassId", studentClass.Class.Name ?? studentClass.Class.Id.ToString() }
            };

            await SendEmail(EmailType.High_Unpaid_Slots_Warning, studentClass.Student.Email, emailParams);

            foreach (var slot in studentClass.Class.Slots)
            {
                await _slotStudentServices.SoftDeleteSlotStudent(slot.Id, studentClass.Student.Id);
            }
            await DeleteStudentClass(studentClass.Class.Id, studentClass.Student.Id);

        }


        private async Task SendPaymentReminder(Models.Models.StudentClass studentClass)
        {
            var emailParams = new Dictionary<string, string>
            {
                { "Name", studentClass.Student.Email },
                { "ClassId", studentClass.Class.Name ?? studentClass.Class.Id.ToString() }
            };

            await SendEmail(EmailType.Slot_Payment_Reminder, studentClass.Student.Email, emailParams);          
        }

        private async Task SendEmail(string emailType, string toAddress, Dictionary<string, string> emailParams)
        {
            var toAddressList = new List<string> { toAddress };
            await _emailServices.SendAsync(emailType, toAddressList, new List<string>(), emailParams);
        }

    }
}


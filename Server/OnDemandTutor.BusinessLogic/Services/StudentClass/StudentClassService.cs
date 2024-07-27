using Mapster;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Class;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
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
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

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

        public StudentClassService(IUnitOfWorkRepository unitOfWork, IAuthServices authService,
            IUserServices userServices, IClassServices classServices, INotificationService notificationService, IHttpContextAccessor HttpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _classServices = classServices;
            _userServices = userServices;
            _httpContextAccessor = HttpContextAccessor;
            _notificationService = notificationService;
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
            return studentClass?.Adapt<GetStudentClassDto>();
        }

        public async Task<CreateStudentClassDto> CreateStudentClassAsync(CreateStudentClassDto studentClassDto)
        {
            var studentClass = studentClassDto.Adapt<Models.Models.StudentClass>();
            var createdStudentClass = await _unitOfWork.StudentClassRepository.AddAsync(studentClass);
            await _unitOfWork.SaveChangesAsync();
            await _notificationService.CreateNotificationAsync(new NotificationCreateDto()
            {
                Content = $"this withdraw with Id{studentClass.Id}  has been created  ",
                IsViewed = true,
                ReceiverId = studentClass.StudentId,
            });
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

            await _notificationService.CreateNotificationAsync(new NotificationCreateDto()
            {
                Content = $"this withdraw with Id{studentClass.Id}  has been deleted  ",
                IsViewed = true,
                ReceiverId = studentClass.StudentId,
            });
            return true;
        }

        public async Task<bool> StudentRatingClassAsync(int classId, int studentId, int Rating, string? Feedback)
        {
            var recordInDB = await _unitOfWork.StudentClassRepository.FirstOrDefaultAsync(st => st.StudentId == studentId && st.ClassId == classId);
            if (recordInDB == null)
            {
                throw new ModelException($"{recordInDB.Id}", "has not found");
            }

            // handle for rating in student class
            recordInDB.Rating = Rating;
            recordInDB.Feedback = Feedback;
            _unitOfWork.StudentClassRepository.Update(recordInDB);
            await _unitOfWork.SaveChangesAsync();

            // handle for update tutor rating 
            var classModel = await _classServices.GetClassByIdAsync(recordInDB.ClassId);
            var tutorId = classModel.TutorId;
            //var tutorModel = await _userServices.GetUserByIdAsync(tutorId);

            //var listClassStudentRating = await _unitOfWork.StudentClassRepository
            //        .Where(ss => ss.ClassId == classModel.Id && ss.Rating.HasValue)
            //        .AverageAsync(l => l.Rating);

            //tutorModel.Rating = (listClassStudentRating + Rating) / 2;
            //await _userServices.UpdateTutorRating(tutorModel);

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
                await _unitOfWork.StudentClassRepository.AddAsync(recordInDb);
                await _unitOfWork.SaveChangesAsync();
            }
            await _notificationService.CreateNotificationAsync(new NotificationCreateDto()
            {
                Content = $"this withdraw with Id{recordInDb.Id}  has been created  ",
                IsViewed = true,
                ReceiverId = recordInDb.StudentId,
            });
            return recordInDb;
        }


        public async Task<bool> DeleteStudentFromStudentClassById(int classId, int userId)
        {
            var studentClass = await _unitOfWork.StudentClassRepository.FirstOrDefaultAsync(sc => sc.ClassId == classId && sc.StudentId == userId);
            if (studentClass == null)
            {
                throw new Exception("StudentClass not found");
            }
            _unitOfWork.StudentClassRepository.Remove(studentClass);
            // studentClass.RecordStatus = RecordStatus.Deleted;
            // _unitOfWork.StudentClassRepository.Update(studentClass);
            await _unitOfWork.SaveChangesAsync();
            await _notificationService.CreateNotificationAsync(new NotificationCreateDto()
            {
                Content = $"this withdraw with Id{studentClass.Id}  has been created  ",
                IsViewed = true,
                ReceiverId = studentClass.StudentId,
            });
            return true;
        }


    }
}


using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.BusinessLogic.Interfaces.TutorDegree;
using OnDemandTutor.BusinessLogic.Interfaces.TutorSubject;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Dtos.TutorDegree;
using OnDemandTutor.Models.Dtos.TutorSubject;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;
using System.Globalization;

namespace OnDemandTutor.BusinessLogic.Services.TutorSubject
{
    public class TutorSubjectService : ITutorSubjectService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly ITutorDegreeService _tutorDegreeService;
        private readonly INotificationService _notificationService;

        public TutorSubjectService(IUnitOfWorkRepository unitOfWork, ITutorDegreeService tutorDegreeService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _tutorDegreeService = tutorDegreeService;
            _notificationService = notificationService;
        }

        public async Task<PagedResult<GetTutorSubjectWithUserAndSubjectDto>> GetTutorSubjectsAsync(PagingModel<QueryTutorSubjectDto> request)
        {
            var pagedTutorSubjects = await _unitOfWork.TutorSubjectRepository.GetTutorSubjects(request);
            return pagedTutorSubjects.Adapt<PagedResult<GetTutorSubjectWithUserAndSubjectDto>>();
        }

        public async Task<GetTutorSubjectDetailDto> GetTutorSubjectByIdAsync(int id)
        {
            var tutorSubjectEntity = await _unitOfWork.TutorSubjectRepository.GetTutorSubjectById(id);
            if (tutorSubjectEntity == null)
            {
                throw new NotFoundException($"TutorSubject with ID {id} not found.");
            }
            var mappedTutorSubject = tutorSubjectEntity.Adapt<GetTutorSubjectDetailDto>();
            mappedTutorSubject.Degrees = await _tutorDegreeService.GetTutorDegreesByTutorIdAndSubjectId(mappedTutorSubject.UserId, mappedTutorSubject.SubjectId);
            return mappedTutorSubject;
        }

        public async Task<GetTutorSubjectDetailDto> CreateTutorSubjectAsync(CreateTutorSubjectDto tutorSubjectDto, GetProfileUserDtos user)
        {
            var tutorSubjectEntity = tutorSubjectDto.Adapt<Models.Models.TutorSubject>();
            tutorSubjectEntity.UserId = user.Id;
            var createdTutorSubjectEntity = await _unitOfWork.TutorSubjectRepository.AddAsync(tutorSubjectEntity);
            foreach (var degree in tutorSubjectDto.Degrees)
            {
                var createDto = degree.Adapt<CreateTutorDegreeDto>();
                createDto.TutorId = user.Id;
                createDto.SubjectId = tutorSubjectDto.SubjectId;
                await _tutorDegreeService.CreateTutorDegreeAsync(createDto);
            }
            createdTutorSubjectEntity.Entity.Status = Models.Enum.TutorSubjectStatus.Pending;
            await _notificationService.CreateNotificationAsync(new CreateNotificationDto
            {
                Content = $"Đã gửi yêu cầu đăng ký môn học thành công. Chúng tôi sẽ phản hồi tới bạn! trong vòng 48h",
                RefUrl = "/tutor/subject",
                ReceiverIds = new List<int> { user.Id },
                RefImageUrl = "/src/assets/logo.png"
            });
            await _unitOfWork.SaveChangesAsync();
            return createdTutorSubjectEntity.Entity.Adapt<GetTutorSubjectDetailDto>();
        }

        public async Task<UpdateTutorSubjectDto> UpdateTutorSubjectAsync(UpdateTutorSubjectDto tutorSubjectDto)
        {
            var existingTutorSubjectEntity = await _unitOfWork.TutorSubjectRepository.FirstOrDefaultAsync(ts => ts.Id == tutorSubjectDto.Id);
            if (existingTutorSubjectEntity == null)
            {
                throw new NotFoundException($"TutorSubject with ID {tutorSubjectDto.Id} not found.");
            }

            existingTutorSubjectEntity = tutorSubjectDto.Adapt(existingTutorSubjectEntity);

            var updatedTutorSubjectEntity = _unitOfWork.TutorSubjectRepository.Update(existingTutorSubjectEntity);
            await _unitOfWork.SaveChangesAsync();
            //Notification
            var tutorSubjectDetail = await GetTutorSubjectByIdAsync(tutorSubjectDto.Id);
            string message = "";
            if (tutorSubjectDto.Status == Models.Enum.TutorSubjectStatus.Approved)
            {
                message = $"Chúc mừng, môn học {tutorSubjectDetail.Subject.Name} mà bạn đăng ký đã được chấp thuận. Giờ đây bạn có thể dạy học, tạo lớp cho môn này trên nền tảng!";
            }
            if (tutorSubjectDto.Status == Models.Enum.TutorSubjectStatus.Rejected)
            {
                message = $"Môn học {tutorSubjectDetail.Subject.Name} mà bạn đăng ký đã bị từ chối. Hãy xem chi tiết tại đây!";
            }
            if (tutorSubjectDto.Status == Models.Enum.TutorSubjectStatus.Disable)
            {
                message = $"Môn học {tutorSubjectDetail.Subject.Name} bạn đang dạy đã bị VÔ HIỆU HÓA. Hãy xem chi tiết tại đây!";
            }
            await _notificationService.CreateNotificationAsync(new CreateNotificationDto
            {
                Content = message,
                RefUrl = "/tutor/subject",
                ReceiverIds = new List<int> { tutorSubjectDetail.UserId },
                RefImageUrl = "/src/assets/logo.png"
            });
            return updatedTutorSubjectEntity.Adapt<UpdateTutorSubjectDto>();
        }

        public async Task<bool> DeleteTutorSubjectAsync(int id)
        {
            var existingTutorSubjectEntity = await _unitOfWork.TutorSubjectRepository.FirstOrDefaultAsync(ts => ts.Id == id);
            if (existingTutorSubjectEntity == null)
            {
                throw new NotFoundException($"TutorSubject with ID {id} not found.");
            }

            _unitOfWork.TutorSubjectRepository.Remove(existingTutorSubjectEntity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
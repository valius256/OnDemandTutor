using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public NotificationService(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<NotificationGetDto>> GetNotificationsAsync(int page, int limit, GetProfileUserDtos user)
        {
            var pagedNotifications = await _unitOfWork.NotificationRepository.GetNotificationByReceiverId(user.Id, page, limit);
            return pagedNotifications.Adapt<PagedResult<NotificationGetDto>>();
        }

        public async Task<NotificationGetDto> GetNotificationByIdAsync(int id)
        {
            var notification = await _unitOfWork.NotificationRepository.GetNotificationWithReceiverByIdAsync(id);
            if (notification == null)
            {
                throw new NotFoundException($"Notification with ID {id} not found.");
            }
            var notificationDto = notification.Adapt<NotificationGetDto>();
            notificationDto.ReceiverName = notification.Receiver?.LastName; // Assuming User has a Name property
            return notificationDto;
        }

        public async Task<NotificationGetDto> CreateNotificationAsync(NotificationCreateDto notificationCreateDto)
        {
            var notificationEntity = notificationCreateDto.Adapt<Models.Models.Notification>();
            await _unitOfWork.NotificationRepository.AddAsync(notificationEntity);
            await _unitOfWork.SaveChangesAsync();
            return notificationEntity.Adapt<NotificationGetDto>();
        }

        public async Task<NotificationGetDto> UpdateViewStatus(int id)
        {
            var existingNotification = await _unitOfWork.NotificationRepository.FirstOrDefaultAsync(n => n.Id == id);
            if (existingNotification == null)
            {
                throw new NotFoundException($"Notification with ID {id} not found.");
            }
            existingNotification.IsViewed = true;
            var updatedNotification = _unitOfWork.NotificationRepository.Update(existingNotification);
            await _unitOfWork.SaveChangesAsync();

            return updatedNotification.Adapt<NotificationGetDto>();
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var existingNotification = await _unitOfWork.NotificationRepository.GetNotificationWithReceiverByIdAsync(id);
            if (existingNotification == null)
            {
                throw new NotFoundException($"Notification with ID {id} not found.");
            }

            _unitOfWork.NotificationRepository.Remove(existingNotification);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}


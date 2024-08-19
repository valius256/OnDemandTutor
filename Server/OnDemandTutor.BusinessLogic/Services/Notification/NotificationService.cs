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

        public async Task<PagedResult<GetNotificationDto>> GetNotificationsAsync(int page, int limit, GetProfileUserDto user)
        {
            var pagedNotifications = await _unitOfWork.NotificationRepository.GetNotificationByReceiverId(user.Id, page, limit);
            return pagedNotifications.Adapt<PagedResult<GetNotificationDto>>();
        }

        public async Task<GetNotificationDto> GetNotificationByIdAsync(int id)
        {
            var notification = await _unitOfWork.NotificationRepository.GetNotificationWithReceiverByIdAsync(id);
            if (notification == null)
            {
                throw new DataNotFoundException($"Notification with ID {id} not found.");
            }
            var notificationDto = notification.Adapt<GetNotificationDto>();
            notificationDto.ReceiverName = notification.Receiver?.LastName; // Assuming User has a Name property
            return notificationDto;
        }

        public async Task CreateNotificationAsync(CreateNotificationDto notificationCreateDto)
        {
            foreach (var receiveId in notificationCreateDto.ReceiverIds)
            {
                var notificationEntity = notificationCreateDto.Adapt<Models.Models.Notification>();
                notificationEntity.ReceiverId = receiveId;
                notificationEntity.IsViewed = false;

                await _unitOfWork.NotificationRepository.AddAsync(notificationEntity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<GetNotificationDto> UpdateViewStatus(int id)
        {
            var existingNotification = await _unitOfWork.NotificationRepository.FirstOrDefaultAsync(n => n.Id == id);
            if (existingNotification == null)
            {
                throw new DataNotFoundException($"Notification with ID {id} not found.");
            }
            existingNotification.IsViewed = true;
            var updatedNotification = _unitOfWork.NotificationRepository.Update(existingNotification);
            await _unitOfWork.SaveChangesAsync();

            return updatedNotification.Adapt<GetNotificationDto>();
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var existingNotification = await _unitOfWork.NotificationRepository.GetNotificationWithReceiverByIdAsync(id);
            if (existingNotification == null)
            {
                throw new DataNotFoundException($"Notification with ID {id} not found.");
            }

            _unitOfWork.NotificationRepository.Remove(existingNotification);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}


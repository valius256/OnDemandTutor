using System;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Paging;
using Mapster;

namespace OnDemandTutor.BusinessLogic.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public NotificationService(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<NotificationGetDto>> GetNotificationsAsync(PagingModel<NotificationGetDto> request)
        {
            var pagedNotifications = await _unitOfWork.NotificationRepository.PagingAsync(request.Adapt<PagingModel<Models.Models.Notification>>());
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
            var createdNotificationEntity = await _unitOfWork.NotificationRepository.AddAsync(notificationEntity);
            await _unitOfWork.SaveChangesAsync();
            return createdNotificationEntity.Adapt<NotificationGetDto>();
        }

        public async Task<NotificationGetDto> UpdateNotificationAsync(NotificationGetDto notificationGetDto)
        {
            var existingNotification = await _unitOfWork.NotificationRepository.GetNotificationWithReceiverByIdAsync(notificationGetDto.Id);
            if (existingNotification == null)
            {
                throw new NotFoundException($"Notification with ID {notificationGetDto.Id} not found.");
            }

            existingNotification = notificationGetDto.Adapt(existingNotification);

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


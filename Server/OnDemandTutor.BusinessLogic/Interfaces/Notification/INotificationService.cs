using System;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Notification
{
    public interface INotificationService
    {
        Task<PagedResult<NotificationGetDto>> GetNotificationsAsync(PagingModel<NotificationGetDto> request);
        Task<NotificationGetDto> GetNotificationByIdAsync(int id);
        Task<NotificationGetDto> CreateNotificationAsync(NotificationCreateDto notificationCreateDto);
        Task<NotificationGetDto> UpdateNotificationAsync(NotificationGetDto notificationGetDto);
        Task<bool> DeleteNotificationAsync(int id);
    }
}


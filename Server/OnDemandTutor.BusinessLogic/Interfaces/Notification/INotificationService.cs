using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Notification
{
    public interface INotificationService
    {
        Task<PagedResult<NotificationGetDto>> GetNotificationsAsync(int page, int limit, GetProfileUserDtos user);
        Task<NotificationGetDto> GetNotificationByIdAsync(int id);
        Task<NotificationGetDto> CreateNotificationAsync(NotificationCreateDto notificationCreateDto);
        Task<NotificationGetDto> UpdateViewStatus(int id);
        Task<bool> DeleteNotificationAsync(int id);
    }
}


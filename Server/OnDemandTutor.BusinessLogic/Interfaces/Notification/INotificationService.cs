using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.Notification;

public interface INotificationService
{
    Task<PagedResult<GetNotificationDto>> GetNotificationsAsync(int page, int limit, GetProfileUserDtos user);
    Task<GetNotificationDto> GetNotificationByIdAsync(int id);
    Task CreateNotificationAsync(CreateNotificationDto notificationCreateDto);
    Task<GetNotificationDto> UpdateViewStatus(int id);
    Task<bool> DeleteNotificationAsync(int id);
}
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<Notification?> GetNotificationWithReceiverByIdAsync(int id);

        Task<PagedResult<Notification>> GetNotificationByReceiverId(int id, int page, int limit);
    }
}


using System;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.IRepository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<Notification> GetNotificationWithReceiverByIdAsync(int id);
    }
}


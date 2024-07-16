using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context; 
        }
        public async Task<PagedResult<Notification>> GetNotificationByReceiverId(int id, int page, int limit)
        {
            return await dbSet
                .Where(n => n.ReceiverId == id)
                .ToNewPagingAsync(page, limit);
        }
        public async Task<Notification> GetNotificationWithReceiverByIdAsync(int id)
        {
            return await _context.Notifications
                .Include(n => n.Receiver)
                .FirstOrDefaultAsync(n => n.Id == id);
        }
    }
}


using System.Collections.Generic;

using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using NewHospitalManagementSystem.Models;

namespace NewHospitalManagementSystem.Repository

{

    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDBContext _context;

        public NotificationRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }
    }
}
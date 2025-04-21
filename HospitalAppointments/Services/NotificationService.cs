using System.Collections.Generic;

using System.Threading.Tasks;

using NewHospitalManagementSystem.Repository;

using NewHospitalManagementSystem.Models;

namespace Hospital_medical_WebAPI.Service

{

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _notificationRepository.AddNotificationAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _notificationRepository.GetAllNotificationsAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            return await _notificationRepository.GetNotificationByIdAsync(id);
        }
    }
}



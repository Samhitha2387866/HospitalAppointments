using System.Collections.Generic;

using System.Threading.Tasks;

using NewHospitalManagementSystem.Models;

namespace NewHospitalManagementSystem.Repository

{

    public interface INotificationRepository
    {
        Task AddNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<Notification> GetNotificationByIdAsync(int id);
    }

}


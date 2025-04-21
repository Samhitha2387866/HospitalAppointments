using System.Collections.Generic;

using System.Threading.Tasks;

using NewHospitalManagementSystem.Models;

namespace Hospital_medical_WebAPI.Service

{

    public interface INotificationService
    {
        Task AddNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<Notification> GetNotificationByIdAsync(int id);
    }

}


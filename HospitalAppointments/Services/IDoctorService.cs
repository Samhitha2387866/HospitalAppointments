using System.Collections.Generic;

using System.Threading.Tasks;

using NewHospitalManagementSystem.Models;

namespace NewHospitalManagementSystem.Service

{

    public interface IDoctorService

    {

        Task<IEnumerable<DoctorRegistration>> GetAllDoctors();

        Task<DoctorRegistration?> GetDoctorById(int id);

        Task<DoctorRegistration> AddDoctor(DoctorRegistration doctorRegistration);

        Task<DoctorRegistration?> UpdateDoctor(int id, DoctorRegistration doctor);

        Task<bool> DeleteDoctor(int id);

    }

}

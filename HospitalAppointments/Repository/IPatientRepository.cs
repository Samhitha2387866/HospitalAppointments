using System.Collections.Generic;

using System.Threading.Tasks;

using NewHospitalManagementSystem.Models;

namespace NewHospitalManagementSystem.Repository

{

    public interface IPatientRepository

    {

        Task<IEnumerable<PatientRegistration>> GetAllAsync();

        Task<PatientRegistration?> GetByIdAsync(int id);

        Task AddAsync(PatientRegistration patient);

        Task UpdateAsync(PatientRegistration patient);

        Task<bool> DeleteAsync(int id);

        Task<PatientRegistration?> GetPatientByContactAsync(string contactNumber);

    }

}

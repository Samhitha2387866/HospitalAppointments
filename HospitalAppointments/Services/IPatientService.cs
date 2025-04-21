using System.Collections.Generic;

using System.Threading.Tasks;

using NewHospitalManagementSystem.Models;

namespace Hospital_medical_WebAPI.Service

{

    public interface IPatientService
    {
        Task<IEnumerable<PatientRegistration>> GetAllPatients();
        Task<PatientRegistration> GetPatientById(int id);
        Task<PatientRegistration> CreatePatient(PatientRegistration patient);
        Task<PatientRegistration> UpdatePatient(int id, PatientRegistration patient);
        Task<bool> DeletePatient(int id);
    }

}

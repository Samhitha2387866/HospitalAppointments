using System.Collections.Generic;

using System.Threading.Tasks;

using NewHospitalManagementSystem.Models;

using NewHospitalManagementSystem.Repository;

namespace Hospital_medical_WebAPI.Service

{

    public class PatientService : IPatientService

    {

        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)

        {

            _patientRepository = patientRepository;

        }

        public async Task<IEnumerable<PatientRegistration>> GetAllPatients()

        {

            return await _patientRepository.GetAllAsync();

        }

        public async Task<PatientRegistration> GetPatientById(int id)

        {

            return await _patientRepository.GetByIdAsync(id);

        }

        public async Task<PatientRegistration> CreatePatient(PatientRegistration patient)

        {

            await _patientRepository.AddAsync(patient);

            return patient;

        }

        public async Task<PatientRegistration> UpdatePatient(int id, PatientRegistration patient)

        {

            await _patientRepository.UpdateAsync(patient);

            return patient;

        }

        public async Task<bool> DeletePatient(int id)

        {

            return await _patientRepository.DeleteAsync(id);

        }

    }

}

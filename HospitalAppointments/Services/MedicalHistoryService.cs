using System.Collections.Generic;

using System.Threading.Tasks;

using NewHospitalManagementSystem.Models;

using NewHospitalManagementSystem.Repository;

namespace Hospital_medical_WebAPI.Service

{

    public class MedicalHistoryService : IMedicalHistoryService

    {

        private readonly IMedicalHistoryRepository _medicalHistoryRepository;

        public MedicalHistoryService(IMedicalHistoryRepository medicalHistoryRepository)

        {

            _medicalHistoryRepository = medicalHistoryRepository;

        }

        public async Task<IEnumerable<MedicalHistory>> GetAllMedicalHistories()

        {

            return await _medicalHistoryRepository.GetAllMedicalHistories();

        }

        public async Task<MedicalHistory> GetMedicalHistoryById(int id)

        {

            return await _medicalHistoryRepository.GetMedicalHistoryById(id);

        }

        public async Task<IEnumerable<MedicalHistory>> GetMedicalHistoriesByPatientId(int patientId)

        {

            return await _medicalHistoryRepository.GetMedicalHistoriesByPatientId(patientId);

        }


        public async Task<MedicalHistory> AddMedicalHistory(MedicalHistory medicalHistory)

        {

            return await _medicalHistoryRepository.AddMedicalHistory(medicalHistory);

        }

        public async Task<MedicalHistory> UpdateMedicalHistory(int id, MedicalHistory medicalHistory)

        {

            return await _medicalHistoryRepository.UpdateMedicalHistory(id, medicalHistory);

        }

        public async Task<bool> DeleteMedicalHistory(int id)

        {

            return await _medicalHistoryRepository.DeleteMedicalHistory(id);

        }

    }

}

using System.Collections.Generic;

using System.Threading.Tasks;

using NewHospitalManagementSystem.Models;

namespace Hospital_medical_WebAPI.Service

{

    public interface IMedicalHistoryService

    {

        Task<IEnumerable<MedicalHistory>> GetAllMedicalHistories();

        Task<MedicalHistory> GetMedicalHistoryById(int id);

        Task<MedicalHistory> AddMedicalHistory(MedicalHistory medicalHistory);

        Task<MedicalHistory> UpdateMedicalHistory(int id, MedicalHistory medicalHistory);

        Task<bool> DeleteMedicalHistory(int id);

        Task<IEnumerable<MedicalHistory>> GetMedicalHistoriesByPatientId(int patientId);

    }

}

using System.Collections.Generic;

using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using NewHospitalManagementSystem.Models;

namespace NewHospitalManagementSystem.Repository

{

    public class MedicalHistoryRepository : IMedicalHistoryRepository

    {

        private readonly ApplicationDBContext _context;

        public MedicalHistoryRepository(ApplicationDBContext context)

        {

            _context = context;

        }

        public async Task<IEnumerable<MedicalHistory>> GetAllMedicalHistories()

        {

            return await _context.medicalHistories.ToListAsync();

        }

        public async Task<MedicalHistory> GetMedicalHistoryById(int id)

        {

            return await _context.medicalHistories.FindAsync(id);

        }

        public async Task<IEnumerable<MedicalHistory>> GetMedicalHistoriesByPatientId(int patientId)

        {

            return await _context.medicalHistories

                .Where(mh => mh.PatientId == patientId)

                .ToListAsync();

        }

        public async Task<MedicalHistory> AddMedicalHistory(MedicalHistory medicalHistory)

        {

            _context.medicalHistories.Add(medicalHistory);

            await _context.SaveChangesAsync();

            return medicalHistory;

        }

        public async Task<MedicalHistory> UpdateMedicalHistory(int id, MedicalHistory medicalHistory)

        {

            _context.medicalHistories.Update(medicalHistory);

            await _context.SaveChangesAsync();

            return medicalHistory;

        }

        public async Task<bool> DeleteMedicalHistory(int id)

        {

            var medicalHistory = await _context.medicalHistories.FindAsync(id);

            if (medicalHistory == null)

            {

                return false;

            }

            _context.medicalHistories.Remove(medicalHistory);

            await _context.SaveChangesAsync();

            return true;

        }

    }

}

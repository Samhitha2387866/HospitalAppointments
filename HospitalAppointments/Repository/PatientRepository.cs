using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using NewHospitalManagementSystem.Models;

namespace NewHospitalManagementSystem.Repository

{

    public class PatientRepository : IPatientRepository

    {

        private readonly ApplicationDBContext _context;

        public PatientRepository(ApplicationDBContext context)

        {

            _context = context;

        }

        public async Task<IEnumerable<PatientRegistration>> GetAllAsync()

        {

            return await _context.Patients.ToListAsync();

        }

        public async Task<PatientRegistration?> GetByIdAsync(int id)

        {

            return await _context.Patients.FindAsync(id);

        }

        public async Task AddAsync(PatientRegistration patient)

        {

            await _context.Patients.AddAsync(patient);

            await _context.SaveChangesAsync();

        }

        public async Task UpdateAsync(PatientRegistration patient)

        {

            _context.Patients.Update(patient);

            await _context.SaveChangesAsync();

        }

        public async Task<bool> DeleteAsync(int id)

        {

            var patient = await _context.Patients.FindAsync(id);

            if (patient != null)

            {

                _context.Patients.Remove(patient);

                await _context.SaveChangesAsync();

                return true;

            }

            return false;

        }

        public async Task<PatientRegistration?> GetPatientByContactAsync(string contactNumber)

        {

            return await _context.Patients.FirstOrDefaultAsync(p => p.ContactNumber == contactNumber);

        }

    }

}

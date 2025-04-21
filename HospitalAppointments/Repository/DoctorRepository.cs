using System.Collections.Generic;

using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using NewHospitalManagementSystem.Models;

namespace NewHospitalManagementSystem.Repository

{

    public class DoctorRepository : IDoctorRepository

    {

        private readonly ApplicationDBContext _context;

        public DoctorRepository(ApplicationDBContext context)

        {

            _context = context;

        }

        public async Task<IEnumerable<DoctorRegistration>> GetAllDoctors()

        {

            return await _context.Doctors.ToListAsync();

        }

        public async Task<DoctorRegistration?> GetDoctorById(int id)

        {

            return await _context.Doctors.FindAsync(id);

        }

        public async Task<DoctorRegistration> AddDoctor(DoctorRegistration doctorRegistration)

        {

            _context.Doctors.Add(doctorRegistration);

            await _context.SaveChangesAsync();

            return doctorRegistration;

        }

        public async Task<DoctorRegistration?> UpdateDoctor(int id, DoctorRegistration doctor)

        {

            var existingDoctor = await _context.Doctors.FindAsync(id);

            if (existingDoctor == null) return null;

            existingDoctor.DoctorName = doctor.DoctorName;

            existingDoctor.Specialization = doctor.Specialization;

            existingDoctor.ContactNumber = doctor.ContactNumber;

            await _context.SaveChangesAsync();

            return existingDoctor;

        }

        public async Task<bool> DeleteDoctor(int id)

        {

            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null) return false;

            _context.Doctors.Remove(doctor);

            await _context.SaveChangesAsync();

            return true;

        }

    }

}

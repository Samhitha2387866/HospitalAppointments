using System.Collections.Generic;

using System.Threading.Tasks;

using NewHospitalManagementSystem.Models;

using NewHospitalManagementSystem.Repository;

namespace NewHospitalManagementSystem.Service

{

    public class DoctorService : IDoctorService

    {

        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)

        {

            _doctorRepository = doctorRepository;

        }

        public async Task<IEnumerable<DoctorRegistration>> GetAllDoctors()

        {

            return await _doctorRepository.GetAllDoctors();

        }

        public async Task<DoctorRegistration?> GetDoctorById(int id)

        {

            return await _doctorRepository.GetDoctorById(id);

        }

        public async Task<DoctorRegistration> AddDoctor(DoctorRegistration doctorRegistration)

        {

            return await _doctorRepository.AddDoctor(doctorRegistration);

        }

        public async Task<DoctorRegistration?> UpdateDoctor(int id, DoctorRegistration doctor)

        {

            return await _doctorRepository.UpdateDoctor(id, doctor);

        }

        public async Task<bool> DeleteDoctor(int id)

        {

            return await _doctorRepository.DeleteDoctor(id);

        }

    }

}

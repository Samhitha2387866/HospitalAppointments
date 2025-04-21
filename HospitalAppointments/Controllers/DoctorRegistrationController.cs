using Microsoft.AspNetCore.Mvc;
using NewHospitalManagementSystem.Models;
using Hospital_medical_WebAPI.Service;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewHospitalManagementSystem.Service;
using System.Text;
using System.Security.Cryptography;

namespace NewHospitalManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorRegistrationController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorRegistrationController(IDoctorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorRegistration>>> GetAllDoctors()
        {
            var doctors = await _service.GetAllDoctors();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorRegistration>> GetDoctorById(int id)
        {
            var doctor = await _service.GetDoctorById(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }

        [HttpPost]
        public async Task<ActionResult<DoctorRegistration>> CreateDoctor([FromBody] DoctorRegistration doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            doctor.Role = "Doctor";
            doctor.PasswordHash = HashPassword(doctor.PasswordHash);

            try
            {
                await _service.AddDoctor(doctor);
                return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.DoctorId }, doctor);
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"An error occurred while saving the entity changes: {errorMessage}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorRegistration updatedDoctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updatedDoctor.DoctorId)
            {
                return BadRequest();
            }

            updatedDoctor.Role = "Doctor";
            updatedDoctor.PasswordHash = HashPassword(updatedDoctor.PasswordHash);

            try
            {
                await _service.UpdateDoctor(id, updatedDoctor);
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"An error occurred while updating the entity changes: {errorMessage}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                await _service.DeleteDoctor(id);
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"An error occurred while deleting the entity changes: {errorMessage}");
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
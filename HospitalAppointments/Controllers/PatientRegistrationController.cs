using Microsoft.AspNetCore.Mvc;
using NewHospitalManagementSystem.Models;
using Hospital_medical_WebAPI.Service;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;

namespace Hospital_Appointment_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientRegistrationController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientRegistrationController(IPatientService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientRegistration>>> GetAllPatients()
        {
            var patients = await _service.GetAllPatients();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientRegistration>> GetPatientById(int id)
        {
            var patient = await _service.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<PatientRegistration>> CreatePatient([FromBody] PatientRegistration patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            patient.PasswordHash = HashPassword(patient.PasswordHash);

            try
            {
                await _service.CreatePatient(patient);
                return CreatedAtAction(nameof(GetPatientById), new { id = patient.PatientId }, patient);
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"An error occurred while saving the entity changes: {errorMessage}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] PatientRegistration updatedPatient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updatedPatient.PatientId)
            {
                return BadRequest();
            }

            updatedPatient.PasswordHash = HashPassword(updatedPatient.PasswordHash);

            try
            {
                await _service.UpdatePatient(id, updatedPatient);
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"An error occurred while updating the entity changes: {errorMessage}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            try
            {
                await _service.DeletePatient(id);
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
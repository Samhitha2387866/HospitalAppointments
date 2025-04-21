using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using NewHospitalManagementSystem.Models;

using Hospital_medical_WebAPI.Service;

//using System.Collections.Generic;

//using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Hospital_medical_WebAPI.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    [Authorize]

    public class MedicalHistoryController : ControllerBase

    {

        private readonly IMedicalHistoryService _service;

        public MedicalHistoryController(IMedicalHistoryService service)

        {

            _service = service;

        }



        [HttpGet]

        [AllowAnonymous]

        public async Task<ActionResult<IEnumerable<MedicalHistory>>> GetMedicalHistories()

        {

            var medicalHistories = await _service.GetAllMedicalHistories();

            return Ok(medicalHistories);

        }


        [HttpGet("{id}")]

        [AllowAnonymous]

        public async Task<ActionResult<MedicalHistory>> GetMedicalHistory(int id)

        {

            var medicalHistory = await _service.GetMedicalHistoryById(id);

            if (medicalHistory == null)

            {

                return NotFound();

            }

            return Ok(medicalHistory);

        }

        [HttpGet("ByPatient/{patientId}")]

        [AllowAnonymous]

        public async Task<ActionResult<IEnumerable<MedicalHistory>>> GetMedicalHistoryByPatientId(int patientId)

        {

            var medicalHistories = await _service.GetMedicalHistoriesByPatientId(patientId);

            if (medicalHistories == null || !medicalHistories.Any())

            {

                return NotFound();

            }

            return Ok(medicalHistories);

        }

        [HttpPut("{id}")]

        [AllowAnonymous]

        public async Task<IActionResult> PutMedicalHistory(int id, MedicalHistory medicalHistory)

        {

            if (id != medicalHistory.PatientId)

            {

                return NotFound();

            }

            await _service.UpdateMedicalHistory(id, medicalHistory);

            return NoContent();

        }

        // POST: api/MedicalHistory

        [HttpPost]

        [AllowAnonymous]

        public async Task<ActionResult<MedicalHistory>> PostMedicalHistory(MedicalHistory medicalHistory)

        {

            var createdMedicalHistory = await _service.AddMedicalHistory(medicalHistory);

            return CreatedAtAction("GetMedicalHistory", new { id = createdMedicalHistory.PatientId }, createdMedicalHistory);

        }

        // DELETE: api/MedicalHistory/5

        [HttpDelete("{id}")]

        [AllowAnonymous]

        public async Task<IActionResult> DeleteMedicalHistory(int id)

        {

            var success = await _service.DeleteMedicalHistory(id);

            if (!success)

            {

                return NotFound();

            }

            return NoContent();

        }

    }

}


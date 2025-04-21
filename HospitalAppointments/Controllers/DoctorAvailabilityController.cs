using Hospital_Appointment_Management_System.Models;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;

using System.Linq;

using System.Security.Claims;

using System.Threading.Tasks;

[ApiController]

[Route("api/[controller]")]

[Authorize]// Only doctors can access

public class DoctorAvailabilityController : ControllerBase

{

    private readonly ApplicationDBContext _context;


    public DoctorAvailabilityController(ApplicationDBContext context)

    {

        _context = context;

    }

    /// <summary>

    /// Add Doctor Availability

    /// </summary>

    [HttpPost("add")]

    public async Task<IActionResult> AddDoctorAvailability([FromBody] DoctorAvailability availability)

    {

        var doctor = await _context.Doctors.FindAsync(availability.DoctorId);

        if (doctor == null)

        {

            return NotFound(new { Message = "Doctor not registered." });

        }

        _context.DoctorAvailabilities.Add(availability);

        await _context.SaveChangesAsync();

        return Ok(new { Message = "Doctor availability added successfully", availability });

    }

    /// <summary>

    /// Get All Doctor Availabilities

    /// </summary>

    [Authorize(Roles = "Doctor")]

    [HttpGet("all")]

    public async Task<ActionResult<IEnumerable<DoctorAvailability>>> GetDoctorAvailabilities()

    {

        return await _context.DoctorAvailabilities.ToListAsync();

    }

    /// <summary>

    /// Get Availability for a Specific Doctor

    /// </summary>

    [HttpGet("{doctorId}")]

    public async Task<ActionResult<IEnumerable<DoctorAvailability>>> GetDoctorAvailability(int doctorId)

    {

        var availabilities = await _context.DoctorAvailabilities

                                           .Where(d => d.DoctorId == doctorId)

                                           .ToListAsync();

        if (!availabilities.Any())

        {

            return NotFound(new { Message = "No availability found for this doctor." });

        }

        return Ok(availabilities);

    }

    /// <summary>

    /// Update Doctor Availability

    /// </summary>

    [HttpPut("update/{id}")]

    public async Task<IActionResult> UpdateDoctorAvailability(int id, [FromBody] DoctorAvailability updatedAvailability)

    {

        var existingAvailability = await _context.DoctorAvailabilities.FindAsync(id);

        if (existingAvailability == null)

        {

            return NotFound(new { Message = "Doctor availability not found." });

        }

        existingAvailability.AvailableDate = updatedAvailability.AvailableDate;

        existingAvailability.StartTime = updatedAvailability.StartTime;

        existingAvailability.EndTime = updatedAvailability.EndTime;

        _context.DoctorAvailabilities.Update(existingAvailability);

        await _context.SaveChangesAsync();

        return Ok(new { Message = "Doctor availability updated successfully", existingAvailability });

    }

    /// <summary>

    /// Delete Doctor Availability

    /// </summary>

    [HttpDelete("delete/{id}")]

    public async Task<IActionResult> DeleteDoctorAvailability(int id)

    {

        var availability = await _context.DoctorAvailabilities.FindAsync(id);

        if (availability == null)

        {

            return NotFound(new { Message = "Doctor availability not found." });

        }

        _context.DoctorAvailabilities.Remove(availability);

        await _context.SaveChangesAsync();

        return Ok(new { Message = "Doctor availability deleted successfully." });

    }

}


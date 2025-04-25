using Hospital_Appointment_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewHospitalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
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
    [AllowAnonymous]

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


    [AllowAnonymous]
    [HttpGet("all")]

    public async Task<ActionResult<IEnumerable<DoctorAvailability>>> GetDoctorAvailabilities()

    {

        return await _context.DoctorAvailabilities.ToListAsync();

    }

    /// <summary>

    /// Get Availability for a Specific Doctor

    /// </summary>

    [HttpGet("{doctorId}")]
    [AllowAnonymous]

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
    [AllowAnonymous]

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
    [AllowAnonymous]

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
    [HttpGet("doctor-details/{dateString}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<DoctorRegistration>>> GetDoctorDetailsByDate(string dateString)
    {
        if (!DateOnly.TryParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            return BadRequest(new { Message = "Invalid date format. Please use dd-MM-yyyy." });

        var doctorDetails = await (from da in _context.DoctorAvailabilities
                                   join d in _context.Doctors on da.DoctorId equals d.DoctorId
                                   where da.AvailableDate == date
                                   select d)
                                   .Distinct()
                                   .ToListAsync();

        if (!doctorDetails.Any())
            return NotFound(new { Message = "No doctors available on this date." });

        return Ok(doctorDetails.Select(d => new { d.DoctorId, d.DoctorName, d.Specialization }));

        //return Ok(doctorDetails);
    }

    // 2. Get available slots for a doctor on a date (excluding booked slots)
    [HttpGet("available-slots/{doctorId}/{dateString}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<object>>> GetAvailableTimeSlots(int doctorId, string dateString)
    {
        if (!DateOnly.TryParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            return BadRequest(new { Message = "Invalid date format. Please use dd-MM-yyyy." });

        var availabilities = await _context.DoctorAvailabilities
            .Where(d => d.DoctorId == doctorId && d.AvailableDate == date)
            .ToListAsync();

        if (!availabilities.Any())
            return NotFound(new { Message = "No availability found for this doctor on the specified date." });

        // Get booked appointment times for this doctor and date
        var bookedTimes = await _context.Appointment
            .Where(a => a.DoctorId == doctorId && a.AppointmentDate == date)
            .Select(a => a.AppointmentTime)
            .ToListAsync();

        // Generate available slots (30 min slots by default)
        var availableSlots = new List<object>();
        foreach (var availability in availabilities)
        {
            var slotStart = availability.StartTime;
            while (slotStart < availability.EndTime)
            {
                var slotEnd = slotStart.Add(TimeSpan.FromMinutes(30));
                if (slotEnd > availability.EndTime)
                    break;

                // If this slot is not booked, add to available slots
                if (!bookedTimes.Contains(slotStart))
                {
                    availableSlots.Add(new
                    {
                        StartTime = slotStart.ToString(@"hh\:mm"),
                        EndTime = slotEnd.ToString(@"hh\:mm")
                    });
                }
                slotStart = slotEnd;
            }
        }

        if (!availableSlots.Any())
            return Ok(new { Message = "No available slots for this doctor on the specified date." });

        return Ok(availableSlots);
    }

}


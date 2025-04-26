using NewHospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewHospitalManagementSystem.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly ApplicationDBContext _context; // only for notifications

    public AppointmentsController(IAppointmentService appointmentService, ApplicationDBContext context)
    {
        _appointmentService = appointmentService;
        _context = context;
    }

    [AllowAnonymous]
    [HttpPost("book")]
    public async Task<IActionResult> BookAppointment([FromBody] AppointmentBookingDto bookingDto)
    {
        try
        {
            var appointment = await _appointmentService.BookAppointment(bookingDto);

            // Create a new notification
            var notification = new Notification
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            var result = new
            {
                appointment.AppointmentId,
                appointment.DoctorId,
                DoctorName = appointment.Doctor?.DoctorName,
                appointment.PatientId,
                PatientName = appointment.Patient?.PatientName,
                appointment.AppointmentDate,
                appointment.AppointmentTime,
                appointment.Status
            };

            return Ok(result);
        }
        catch (InvalidAppointmentDateException ex) { return BadRequest(ex.Message); }
        catch (DoctorNotAvailableException ex) { return NotFound(ex.Message); }
        catch (System.Exception ex) { return StatusCode(500, ex.Message); }
    }

    [HttpGet("view")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<object>>> GetAppointmentsById()
    {
        var appointments = await _appointmentService.GetAppointmentsById();
        var result = appointments.Select(a => new
        {
            a.AppointmentId,
            a.DoctorId,
            DoctorName = a.Doctor?.DoctorName,
            a.PatientId,
            PatientName = a.Patient?.PatientName,
            a.AppointmentDate,
            a.AppointmentTime,
            a.Status
        });
        return Ok(result);
    }

    [HttpGet("view/sorted")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<object>>> GetAppointments()
    {
        var appointments = await _appointmentService.GetAppointments();
        var result = appointments.Select(a => new
        {
            a.AppointmentId,
            a.DoctorId,
            DoctorName = a.Doctor?.DoctorName,
            a.PatientId,
            PatientName = a.Patient?.PatientName,
            a.AppointmentDate,
            a.AppointmentTime,
            a.Status
        });
        return Ok(result);
    }

    [HttpGet("view/byDoctor/{doctorId}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<object>>> GetAppointmentsByDoctorId(int doctorId)
    {
        var appointments = await _appointmentService.GetAppointmentsByDoctorId(doctorId);
        var result = appointments.Select(a => new
        {
            a.AppointmentId,
            a.DoctorId,
            DoctorName = a.Doctor?.DoctorName,
            a.PatientId,
            PatientName = a.Patient?.PatientName,
            a.AppointmentDate,
            a.AppointmentTime,
            a.Status
        });
        return Ok(result);
    }

    [HttpDelete("cancel/{appointmentId}")]
    [AllowAnonymous]
    public async Task<IActionResult> CancelAppointment(int appointmentId)
    {
        var success = await _appointmentService.CancelAppointment(appointmentId);
        if (!success) return NotFound("Appointment not found.");
        return Ok("Appointment canceled successfully.");
    }

    [HttpGet("ByPatient/{patientId}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<object>>> GetAppointmentsByPatient(int patientId)
    {
        var appointments = await _appointmentService.GetAppointmentsByPatientId(patientId);
        if (appointments == null || !appointments.Any())
            return NotFound();
        var result = appointments.Select(a => new
        {
            a.AppointmentId,
            a.DoctorId,
            doctorName = a.Doctor?.DoctorName ?? "Unknown",
            specialization = a.Doctor?.Specialization ?? "",
            a.PatientId,
            patientName = a.Patient?.PatientName ?? "Unknown",
            a.AppointmentDate,
            a.AppointmentTime,
            a.Status
        });
        return Ok(result);
    }
    [HttpDelete("ByPatient/{patientId}/{appointmentId}")]
    [AllowAnonymous]
    public async Task<IActionResult> CancelAppointmentForPatient(int patientId, int appointmentId)
    {
        var success = await _appointmentService.CancelAppointmentForPatient(appointmentId, patientId);
        if (!success)
            return NotFound();
        return NoContent();
    }
}

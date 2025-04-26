using NewHospitalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDBContext _context;

    public AppointmentRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Appointment> BookAppointmentAsync(Appointment appointment)
    {
        _context.Appointment.Add(appointment);
        await _context.SaveChangesAsync();

        // Load navigation properties
        await _context.Entry(appointment).Reference(a => a.Doctor).LoadAsync();
        await _context.Entry(appointment).Reference(a => a.Patient).LoadAsync();

        return appointment;
    }

    public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
    {
        return await _context.Appointment
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
    {
        return await _context.Appointment
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .OrderBy(a => a.AppointmentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId)
    {
        return await _context.Appointment
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .Where(a => a.DoctorId == doctorId)
            .OrderBy(a => a.AppointmentDate)
            .ThenBy(a => a.AppointmentTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId)
    {
        return await _context.Appointment
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .Where(a => a.PatientId == patientId)
            .OrderBy(a => a.AppointmentDate)
            .ThenBy(a => a.AppointmentTime)
            .ToListAsync();
    }

    public async Task<bool> CancelAppointmentAsync(int appointmentId)
    {
        var appointment = await _context.Appointment.FindAsync(appointmentId);
        if (appointment == null)
            return false;
        _context.Appointment.Remove(appointment);
        await _context.SaveChangesAsync();
        return true;
    }

    // New helper methods for clean architecture!
    public async Task<bool> PatientExistsAsync(int patientId)
    {
        return await _context.Patients.AnyAsync(p => p.PatientId == patientId);
    }

    public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateOnly date, TimeSpan time)
    {
        return await _context.DoctorAvailabilities
            .AnyAsync(da => da.DoctorId == doctorId &&
                            da.AvailableDate == date &&
                            da.StartTime <= time &&
                            da.EndTime >= time);
    }

    public async Task<bool> HasOverlappingAppointment(int doctorId, DateOnly date, TimeSpan startTime, TimeSpan endTime)
    {
        var appointments = await _context.Appointment
            .Where(a => a.DoctorId == doctorId && a.AppointmentDate == date)
            .ToListAsync();

        return appointments.Any(a =>
            (a.AppointmentTime >= startTime && a.AppointmentTime < endTime) ||
            (a.AppointmentTime.Add(TimeSpan.FromMinutes(30)) > startTime &&
             a.AppointmentTime.Add(TimeSpan.FromMinutes(30)) <= endTime)
        );
    }
    public async Task<bool> CancelAppointmentForPatientAsync(int appointmentId, int patientId)
    {
        var appt = await _context.Appointment
            .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId && a.PatientId == patientId);

        if (appt == null)
            return false;

        // Also remove any related notifications if needed
        var notifications = _context.Notifications.Where(n => n.AppointmentId == appointmentId);
        _context.Notifications.RemoveRange(notifications);

        _context.Appointment.Remove(appt);
        await _context.SaveChangesAsync();
        return true;
    }
}

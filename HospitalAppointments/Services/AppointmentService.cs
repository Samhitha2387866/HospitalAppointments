using NewHospitalManagementSystem.Models;

using Microsoft.EntityFrameworkCore;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using NewHospitalManagementSystem.Exceptions;

public class AppointmentService : IAppointmentService

{

    private readonly ApplicationDBContext _context;

    public AppointmentService(ApplicationDBContext context)

    {

        _context = context;

    }

    public async Task<Appointment> BookAppointment(AppointmentBookingDto bookingDto)

    {

        // Check if the appointment date is before today

        if (bookingDto.AppointmentDate < DateOnly.FromDateTime(DateTime.Today))

        {

            throw new InvalidAppointmentDateException("Appointment should be booked from today onwards.");

        }

        // Check if the patient is registered

        var patient = await _context.Patients.FindAsync(bookingDto.PatientId);

        if (patient == null)

        {

            // Patient is not registered

            return null;

        }

        // Check if the doctor is available at the requested time

        var doctorAvailability = await _context.DoctorAvailabilities.Where(da => da.DoctorId == bookingDto.DoctorId &&

                         da.AvailableDate == bookingDto.AppointmentDate &&

                         da.StartTime <= bookingDto.AppointmentTime &&

                         da.EndTime >= bookingDto.AppointmentTime)

            .FirstOrDefaultAsync();

        if (doctorAvailability == null)

        {

            // Doctor is not available at the requested time

            throw new DoctorNotAvailableException("Doctor is not available at this time.");

        }

        // Calculate the appointment end time in memory

        var appointmentStartTime = bookingDto.AppointmentTime;

        var appointmentEndTime = bookingDto.AppointmentTime.Add(TimeSpan.FromMinutes(30));

        // Retrieve all appointments for the doctor on the requested date

        var doctorAppointments = await _context.Appointment

            .Where(a => a.DoctorId == bookingDto.DoctorId && a.AppointmentDate == bookingDto.AppointmentDate)

            .ToListAsync();

        // Check for overlapping appointments

        var overlappingAppointment = doctorAppointments

            .Any(a => (a.AppointmentTime >= appointmentStartTime && a.AppointmentTime < appointmentEndTime) ||

                      (a.AppointmentTime.Add(TimeSpan.FromMinutes(30)) > appointmentStartTime && a.AppointmentTime.Add(TimeSpan.FromMinutes(30)) <= appointmentEndTime));

        if (overlappingAppointment)

        {

            // Doctor already has an appointment within the 30-minute window

            throw new DoctorNotAvailableException("Doctor already has an appointment within the 30-minute window.");

        }

        // Create a new appointment

        var appointment = new Appointment

        {

            DoctorId = bookingDto.DoctorId,

            PatientId = bookingDto.PatientId,

            AppointmentDate = bookingDto.AppointmentDate,

            AppointmentTime = bookingDto.AppointmentTime,

            Status = "Scheduled"

        };

        _context.Appointment.Add(appointment);

        await _context.SaveChangesAsync();

        // Load the related doctor and patient information

        await _context.Entry(appointment).Reference(a => a.Doctor).LoadAsync();

        await _context.Entry(appointment).Reference(a => a.Patient).LoadAsync();

        return appointment;

    }

    public async Task<bool> CancelAppointment(int appointmentId)

    {

        var appointment = await _context.Appointment.FindAsync(appointmentId);

        if (appointment == null)

        {

            return false;

        }

        _context.Appointment.Remove(appointment);

        await _context.SaveChangesAsync();

        return true;

    }

    public async Task<IEnumerable<Appointment>> GetAppointments()

    {

        return await _context.Appointment

                             .Include(a => a.Doctor)

                             .Include(a => a.Patient)

                             .OrderBy(a => a.AppointmentDate)

                             .ThenBy(a => a.AppointmentTime)

                             .ToListAsync();

    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorId(int doctorId)

    {

        return await _context.Appointment

                             .Include(a => a.Doctor)

                             .Include(a => a.Patient)

                             .Where(a => a.DoctorId == doctorId)

                             .OrderBy(a => a.AppointmentDate)

                             .ThenBy(a => a.AppointmentTime)

                             .ToListAsync();

    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsById()

    {

        return await _context.Appointment

                             .Include(a => a.Doctor)

                             .Include(a => a.Patient)

                             .OrderBy(a => a.AppointmentId)

                             .ToListAsync();

    }

}


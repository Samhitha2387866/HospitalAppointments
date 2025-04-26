using NewHospitalManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewHospitalManagementSystem.Exceptions;
using System;
using System.Linq;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repo;

    public AppointmentService(IAppointmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<Appointment> BookAppointment(AppointmentBookingDto bookingDto)
    {
        if (bookingDto.AppointmentDate < DateOnly.FromDateTime(DateTime.Today))
            throw new InvalidAppointmentDateException("Appointment should be booked from today onwards.");

        if (!await _repo.PatientExistsAsync(bookingDto.PatientId))
            throw new Exception("Patient is not registered.");

        if (!await _repo.IsDoctorAvailableAsync(bookingDto.DoctorId, bookingDto.AppointmentDate, bookingDto.AppointmentTime))
            throw new DoctorNotAvailableException("Doctor is not available at this time.");

        var appointmentStartTime = bookingDto.AppointmentTime;
        var appointmentEndTime = bookingDto.AppointmentTime.Add(TimeSpan.FromMinutes(30));

        if (await _repo.HasOverlappingAppointment(bookingDto.DoctorId, bookingDto.AppointmentDate, appointmentStartTime, appointmentEndTime))
            throw new DoctorNotAvailableException("Doctor already has an appointment within the 30-minute window.");

        var appointment = new Appointment
        {
            DoctorId = bookingDto.DoctorId,
            PatientId = bookingDto.PatientId,
            AppointmentDate = bookingDto.AppointmentDate,
            AppointmentTime = bookingDto.AppointmentTime,
            Status = "Scheduled"
        };

        var created = await _repo.BookAppointmentAsync(appointment);
        return created;
    }

    public async Task<bool> CancelAppointment(int appointmentId)
    {
        return await _repo.CancelAppointmentAsync(appointmentId);
    }

    public async Task<IEnumerable<Appointment>> GetAppointments()
    {
        return await _repo.GetAllAppointmentsAsync();
    }
    public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorId(int doctorId)
    {
        return await _repo.GetAppointmentsByDoctorIdAsync(doctorId);
    }
    public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientId(int patientId)
    {
        return await _repo.GetAppointmentsByPatientIdAsync(patientId);
    }
    public async Task<IEnumerable<Appointment>> GetAppointmentsById()
    {
        return await _repo.GetAllAppointmentsAsync();
    }
    public async Task<bool> CancelAppointmentForPatient(int appointmentId, int patientId)
    {
        return await _repo.CancelAppointmentForPatientAsync(appointmentId, patientId);
    }
}

using NewHospitalManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public interface IAppointmentRepository
{
    Task<Appointment> BookAppointmentAsync(Appointment appointment);
    Task<Appointment> GetAppointmentByIdAsync(int appointmentId);
    Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
    Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId);
    Task<bool> CancelAppointmentAsync(int appointmentId);
    Task<bool> PatientExistsAsync(int patientId);
    Task<bool> CancelAppointmentForPatientAsync(int appointmentId, int patientId);

    Task<bool> IsDoctorAvailableAsync(int doctorId, DateOnly date, TimeSpan time);
    Task<bool> HasOverlappingAppointment(int doctorId, DateOnly date, TimeSpan startTime, TimeSpan endTime);
}

using System.Collections.Generic;
using System.Threading.Tasks;
using NewHospitalManagementSystem.Models;

public interface IAppointmentService
{
    Task<Appointment> BookAppointment(AppointmentBookingDto bookingDto);
    Task<bool> CancelAppointment(int appointmentId);
    Task<IEnumerable<Appointment>> GetAppointments();
    Task<IEnumerable<Appointment>> GetAppointmentsByDoctorId(int doctorId);
    Task<IEnumerable<Appointment>> GetAppointmentsByPatientId(int patientId);
    Task<IEnumerable<Appointment>> GetAppointmentsById();
    Task<bool> CancelAppointmentForPatient(int appointmentId, int patientId);
}

using Hospital_Appointment_Management_System.Models;

using NewHospitalManagementSystem.Models;

using System.Collections.Generic;

using System.Threading.Tasks;

public interface IAppointmentService
{
    Task<Appointment> BookAppointment(AppointmentBookingDto bookingDto);
    Task<IEnumerable<Appointment>> GetAppointmentsById();
    Task<IEnumerable<Appointment>> GetAppointments();
    Task<IEnumerable<Appointment>> GetAppointmentsByDoctorId(int doctorId);
    Task<bool> CancelAppointment(int appointmentId);
}

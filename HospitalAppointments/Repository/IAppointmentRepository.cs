using NewHospitalManagementSystem.Models;

using System.Collections.Generic;

using System.Threading.Tasks;

public interface IAppointmentRepository

{

    Task<Appointment> BookAppointmentAsync(Appointment appointment);

    Task<Appointment> GetAppointmentByIdAsync(int appointmentId);

    Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();

    Task<bool> UpdateAppointmentStatusAsync(int appointmentId, string status);

}


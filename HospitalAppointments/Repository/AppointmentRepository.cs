using NewHospitalManagementSystem.Models;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;

using System.Threading.Tasks;

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

            .ToListAsync();

    }

    public async Task<bool> UpdateAppointmentStatusAsync(int appointmentId, string status)

    {

        var appointment = await _context.Appointment.FindAsync(appointmentId);

        if (appointment == null)

        {

            return false;

        }

        appointment.Status = status;

        await _context.SaveChangesAsync();

        return true;

    }

}


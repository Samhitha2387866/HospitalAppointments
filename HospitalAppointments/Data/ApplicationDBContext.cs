using NewHospitalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using DoctorRegistration = NewHospitalManagementSystem.Models.DoctorRegistration;
using PatientRegistration = NewHospitalManagementSystem.Models.PatientRegistration;
using Hospital_Appointment_Management_System.Models;
public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
        try
        {
            Console.WriteLine("Attempting to migrate the database...");
            Database.Migrate();
            Console.WriteLine("Database migration completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
            throw;
        }
    }
    protected ApplicationDBContext()
    {

    }
    public DbSet<PatientRegistration> Patients { get; set; }
    public DbSet<DoctorRegistration> Doctors { get; set; }
    public DbSet<Appointment> Appointment { get; set; }
    public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<MedicalHistory> medicalHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DoctorAvailability>().Property(e => e.AvailableDate).HasConversion(v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v));
        modelBuilder.Entity<Appointment>().Property(e => e.AppointmentDate).HasConversion(v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v));
        modelBuilder.Entity<PatientRegistration>().Property(e => e.DateOfBirth).HasConversion(v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v));
        modelBuilder.Entity<Notification>().Property(e => e.AppointmentDate).HasConversion(v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v));
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<MedicalHistory>().Property(e => e.VisitDate).HasConversion(v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v));
    }
}

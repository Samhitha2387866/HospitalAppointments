using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Hospital_Appointment_Management_System.Models
{
    public class DoctorAvailability
    {
        public int Id { get; set; }
        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }
        public DateOnly AvailableDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}

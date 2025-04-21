using System;
using System.ComponentModel.DataAnnotations;
namespace NewHospitalManagementSystem.Models
{
    public class AppointmentBookingDto
    {
        [Required(ErrorMessage = "Doctor ID is required.")]
        public int DoctorId { get; set; }
        [Required(ErrorMessage = "Patient ID is required.")]
        public int PatientId { get; set; }
        [Required(ErrorMessage = "Appointment date is required.")]
        public DateOnly AppointmentDate { get; set; }
        [Required(ErrorMessage = "Appointment time is required.")]
        public TimeSpan AppointmentTime { get; set; }
    }
}


using System;

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

using NewHospitalManagementSystem.Models;

namespace NewHospitalManagementSystem.Models

{

    public class Appointment

    {

        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Doctor ID is required.")]

        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]

        public DoctorRegistration Doctor { get; set; } // Navigation property

        [Required(ErrorMessage = "Patient ID is required.")]

        public int PatientId { get; set; }

        [ForeignKey("PatientId")]

        public PatientRegistration Patient { get; set; } // Navigation property

        [Required(ErrorMessage = "Appointment date is required.")]

        public DateOnly AppointmentDate { get; set; }

        [Required(ErrorMessage = "Appointment time is required.")]

        public TimeSpan AppointmentTime { get; set; }

        public string Status { get; set; } = "Scheduled"; // Default status

    }

}


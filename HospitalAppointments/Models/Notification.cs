namespace NewHospitalManagementSystem.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
    }
}


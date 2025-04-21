namespace NewHospitalManagementSystem.Exceptions

{

    public class InvalidAppointmentDateException : Exception

    {

        public InvalidAppointmentDateException() : base() { }

        public InvalidAppointmentDateException(string message) : base(message) { }

        public InvalidAppointmentDateException(string message, Exception innerException) : base(message, innerException) { }

    }

}


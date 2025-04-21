using System;

namespace NewHospitalManagementSystem.Exceptions

{

    public class BadRequestException : Exception

    {

        public BadRequestException(string message) : base(message) { }

    }

}

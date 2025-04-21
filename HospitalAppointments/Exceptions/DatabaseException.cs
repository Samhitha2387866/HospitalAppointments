using System;

namespace NewHospitalManagementSystem.Exceptions

{

    public class DatabaseException : Exception

    {

        public DatabaseException(string message) : base(message) { }

    }

}

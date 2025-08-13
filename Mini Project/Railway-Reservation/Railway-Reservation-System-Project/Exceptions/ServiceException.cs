using System;

namespace Railway_Reservation_System_Project.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message) { }

        public ServiceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}

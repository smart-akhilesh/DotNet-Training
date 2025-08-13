using System;

namespace Railway_Reservation_System_Project.Exceptions
{
    public class NotFoundException : ServiceException
    {
        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}

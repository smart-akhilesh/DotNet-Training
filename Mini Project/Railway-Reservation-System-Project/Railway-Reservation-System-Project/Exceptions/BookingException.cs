using System;

namespace Railway_Reservation_System_Project.Exceptions
{
    public class BookingException : ServiceException
    {
        public BookingException(string message) : base(message) { }

        public BookingException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}

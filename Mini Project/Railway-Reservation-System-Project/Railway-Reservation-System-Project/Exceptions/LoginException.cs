using System;

namespace Railway_Reservation_System_Project.Exceptions
{
    public class LoginException : ServiceException
    {
        public LoginException(string message) : base(message) { }
        public LoginException(string message, Exception inner) : base(message, inner) { }
    }
}

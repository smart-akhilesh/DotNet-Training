using System;

namespace Railway_Reservation_System_Project.Exceptions
{
    public class ReportGenerationException : ServiceException
    {
        public ReportGenerationException(string message) : base(message) { }
        public ReportGenerationException(string message, Exception inner) : base(message, inner) { }
    }
}

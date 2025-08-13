using System;

namespace Railway_Reservation_System_Project.Exceptions
{
    public class TrainOperationException : ServiceException
    {
        public TrainOperationException(string message) : base(message) { }
        public TrainOperationException(string message, Exception inner) : base(message, inner) { }
    }
}

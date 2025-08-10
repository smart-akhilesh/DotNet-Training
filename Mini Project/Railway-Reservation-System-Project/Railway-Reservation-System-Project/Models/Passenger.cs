using System;

namespace Railway_Reservation_System_Project.Models
{
    public class Passenger
    {
        public int PassengerId { get; set; }
        public int BookingId { get; set; }
        public string PassengerName { get; set; }  
        public int Age { get; set; }
        public string Gender { get; set; }
        public string SeatNo { get; set; }        
        public string BerthType { get; set; }      
        public string Status { get; set; }         

        public bool Isdeleted { get; set; }
    }
}

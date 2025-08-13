using System;
using System.Collections.Generic;

namespace Railway_Reservation_System_Project.Models.DTO
{
    public class BookingDetailsDTO
    {
        public Booking Booking { get; set; }
        public List<Passenger> Passengers { get; set; } = new List<Passenger>();
    }

}

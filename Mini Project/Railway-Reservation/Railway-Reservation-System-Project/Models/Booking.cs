using System;

namespace Railway_Reservation_System_Project.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public string PNR { get; set; }
        public int CustomerId { get; set; }
        public string TrainNo { get; set; }
        public string ClassType { get; set; } 
        public DateTime TravelDate { get; set; }
        public int TotalPassengers { get; set; }
        public decimal TotalAmount { get; set; } 
        public DateTime BookingDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}

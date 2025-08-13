using System;


    namespace Railway_Reservation_System_Project.Models
    {
        public class BookingRevenueReport
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int TotalBookings { get; set; }
            public decimal TotalRevenue { get; set; }
        }
    }



using System;

namespace Railway_Reservation_System_Project.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } 
        public int Age { get; set; }
        public string Phone { get; set; }
        public string EmailId { get; set; } 
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
    }
}

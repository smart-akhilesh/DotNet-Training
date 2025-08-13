using System;


namespace Railway_Reservation_System_Project.Models
{
    public class CancellationReport
    {
        public int CancellationId { get; set; }
        public int BookingId { get; set; }
        public string TrainNo { get; set; }
        public int PassengerId { get; set; }
        public decimal RefundAmount { get; set; }
        public DateTime CancellationDate { get; set; }
        public string RefundStatus { get; set; }
    }
}

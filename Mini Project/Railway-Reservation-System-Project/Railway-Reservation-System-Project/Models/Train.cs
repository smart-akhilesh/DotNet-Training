using System;


namespace Railway_Reservation_System_Project.Models
{
    public class Train
    {
        public string TrainNo { get; set; }
        public string TrainName { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public int SleeperSeats { get; set; }
        public int AC3Seats { get; set; }
        public int AC2Seats { get; set; }
        public decimal SleeperPrice { get; set; }
        public decimal AC3Price { get; set; }
        public decimal AC2Price { get; set; }
    }
}

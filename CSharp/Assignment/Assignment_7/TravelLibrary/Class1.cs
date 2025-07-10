using System;

namespace TravelLibrary
{
    public class ConcessionCalculator
    {
        public void CalculateConcession(string name, int age, double fare)
        {
            string message;

            if (age <= 5)
                message = $"{name}, Little Champs - Free Ticket";
            else if (age > 60)
                message = $"{name}, Senior Citizen - Fare after concession: {fare * 0.7}";
            else
                message = $"{name}, Ticket Booked - Fare: {fare}";

            Console.WriteLine(message);
        }
    }
}

using System;
using System.Collections.Generic;
using Railway_Reservation_System_Project.Models;
using Railway_Reservation_System_Project.Models.DTO;

namespace Railway_Reservation_System_Project.Repositories
{
    public interface ICustomerRepository
    {
        int RegisterCustomer(Customer customer);

        bool Login(string email, string password, out int customerId);

        List<Train> ViewTrainsByRoute(string source, string destination, DateTime travelDate);

        (int availableSeats, decimal price) GetAvailableSeatsAndPrice(string trainNo, DateTime travelDate, string classType);

        string BookTicket(int customerId, string trainNo, string classType, DateTime travelDate,
                          int totalPassengers, string paymentMethod, List<Passenger> passengers);

        BookingDetailsDTO ViewBookingDetailsByPNR(string pnr);

        decimal CancelPassengerTicket(string pnr, int passengerId);
    }
}

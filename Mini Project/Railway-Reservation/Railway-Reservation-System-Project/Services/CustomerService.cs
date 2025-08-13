using System;
using System.Collections.Generic;
using Railway_Reservation_System_Project.Models;
using Railway_Reservation_System_Project.Models.DTO;
using Railway_Reservation_System_Project.Repositories;
using Railway_Reservation_System_Project.Exceptions; 

namespace Railway_Reservation_System_Project.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public int RegisterCustomer(Customer customer)
        {
            try
            {
                return _customerRepository.RegisterCustomer(customer);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to register customer.", ex);
            }
        }

        public bool Login(string email, string password, out int customerId)
        {
            try
            {
                return _customerRepository.Login(email, password, out customerId);
            }
            catch (Exception ex)
            {
                throw new LoginException("Login failed.", ex);
            }
        }

 
        public List<Train> ViewTrainsByRoute(string source, string destination, DateTime travelDate)
        {
            try
            {
                return _customerRepository.ViewTrainsByRoute(source, destination, travelDate);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to retrieve trains by route.", ex);
            }
        }

        public (int availableSeats, decimal price) GetAvailableSeatsAndPrice(string trainNo, DateTime travelDate, string classType)
        {
            try
            {
                return _customerRepository.GetAvailableSeatsAndPrice(trainNo, travelDate, classType);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to retrieve available seats and price.", ex);
            }
        }

  
        public string BookTicket(int customerId, string trainNo, string classType, DateTime travelDate,
                                  int totalPassengers, string paymentMethod, List<Passenger> passengers)
        {
            try
            {
                return _customerRepository.BookTicket(customerId, trainNo, classType, travelDate, totalPassengers, paymentMethod, passengers);
            }
            catch (Exception ex)
            {
                throw new BookingException("Ticket booking failed.", ex);
            }
        }

        public BookingDetailsDTO ViewBookingDetailsByPNR(string pnr)
        {
            try
            {
                var booking = _customerRepository.ViewBookingDetailsByPNR(pnr);

                if (booking == null)
                {
                    throw new NotFoundException($"No booking found for PNR: {pnr}");
                }

                return booking;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Failed to retrieve booking details for PNR: {pnr}", ex);
            }
        }


        public decimal CancelPassengerTicket(string pnr, int passengerId)
        {
            try
            {
                return _customerRepository.CancelPassengerTicket(pnr, passengerId);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Ticket cancellation failed.", ex);
            }
        }
    }
}

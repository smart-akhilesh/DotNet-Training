using System;
using System.Collections.Generic;
using Railway_Reservation_System_Project.Repositories;
using Railway_Reservation_System_Project.Services;
using Railway_Reservation_System_Project.Exceptions;
using Railway_Reservation_System_Project.Models;
using Railway_Reservation_System_Project.Models.DTO;
using Railway_Reservation_System_Project.Utils;

namespace Railway_Reservation_System_Project.Client
{
    public class CustomerClient
    {
        public static void Main(string[] args)
        {
          
            Console.WriteLine("==== Railway Reservation Customer Console ====\n");

            ICustomerRepository customerRepo = new CustomerRepository();
            ICustomerService customerService = new CustomerService(customerRepo);

            int loggedInCustomerId = -1;

            while (true)
            {
                Console.WriteLine("\n--- Welcome ---");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");

                int firstChoice = InputHelper.ReadInt("Enter your choice: ");
                if (firstChoice == 1)
                {
                   
                    while (true)
                    {
                        try
                        {
                            string emailId = InputHelper.ReadString("Email: ");
                            string password = InputHelper.ReadString("Password: ");

                            if (customerService.Login(emailId, password, out loggedInCustomerId))
                            {
                                Console.WriteLine($" Login successful. CustomerId = {loggedInCustomerId}\n");
                                break;
                            }
                            else
                            {
                                Console.WriteLine(" Invalid credentials. Try again.\n");
                            }
                        }
                        catch (LoginException lex)
                        {
                            Console.WriteLine($"Login error: {lex.Message}");
                        }
                        catch (ServiceException sex)
                        {
                            Console.WriteLine($"Service error during login: {sex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Unexpected error: {ex.Message}");
                        }
                    }
                    break; 
                }
                else if (firstChoice == 2)
                {
                    try
                    {
                        RegisterCustomer(customerService);
                    }
                    catch (ServiceException sex)
                    {
                        Console.WriteLine($"Service error during registration: {sex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Unexpected error: {ex.Message}");
                    }
                }
                else if (firstChoice == 3)
                {
                    Console.WriteLine("Exiting. Goodbye!");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid option. Try again.");
                }
            }

            while (true)
            {
                Console.WriteLine("\n--- Customer Menu ---");
                Console.WriteLine("1. Search Trains");
                Console.WriteLine("2. Book Ticket");
                Console.WriteLine("3. View Booking by PNR");
                Console.WriteLine("4. Cancel Passenger Ticket");
                Console.WriteLine("5. Logout");

                int choice = InputHelper.ReadInt("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            SearchTrains(customerService);
                            break;
                        case 2:
                            BookTicket(customerService, loggedInCustomerId);
                            break;
                        case 3:
                            ViewBookingByPNR(customerService);
                            break;
                        case 4:
                            CancelPassengerTicket(customerService);
                            break;
                        case 5:
                            Console.WriteLine("Logging out. Goodbye!");
                            return;
                        default:
                            Console.WriteLine("Invalid option. Try again.");
                            break;
                    }
                }
                catch (LoginException ex)
                {
                    Console.WriteLine($" Login error: {ex.Message}");
                }
                catch (NotFoundException ex)
                {
                    Console.WriteLine($" Not found: {ex.Message}");
                }
                catch (BookingException ex)
                {
                    Console.WriteLine($" Booking error: {ex.Message}");
                }
                catch (ServiceException ex)
                {
                    Console.WriteLine($"Service error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }


        private static void RegisterCustomer(ICustomerService service)
        {
            Console.WriteLine("\n--- Register New Customer ---");
            string name = InputHelper.ReadString("Name: ");
            int age = InputHelper.ReadInt("Age: ");
            string phone = InputHelper.ReadString("Phone: ");
            string emailId = InputHelper.ReadString("Email: ");
            string password = InputHelper.ReadString("Password: ");

            Customer newCustomer = new Customer
            {
                CustomerName = name,
                Age = age,
                Phone = phone,
                EmailId = emailId,
                Password = password,
                IsDeleted = false
            };

            int customerId = service.RegisterCustomer(newCustomer);
            if (customerId > 0)
                Console.WriteLine("Registration successful! You can now log in.");
            else
                Console.WriteLine("Registration failed.");
        }

        private static void SearchTrains(ICustomerService service)
        {
            string source = InputHelper.ReadString("Source: ");
            string destination = InputHelper.ReadString("Destination: ");
            DateTime travelDate = InputHelper.ReadDate("Travel Date (yyyy-MM-dd): ");

            var trains = service.ViewTrainsByRoute(source, destination, travelDate);
            if (trains == null || trains.Count == 0)
            {
                Console.WriteLine("No trains found for this route.");
                return;
            }

            Console.WriteLine("\nAvailable Trains:");
            Console.WriteLine("TrainNo  TrainName  Source  Destination  SleeperPrice  AC3Price  AC2Price");

            foreach (var t in trains)
            {
                Console.WriteLine(t.TrainNo + "  " + t.TrainName + "  " + t.Source + "  " + t.Destination +
                                  "  " + t.SleeperPrice + "  " + t.AC3Price + "  " + t.AC2Price);
                Console.WriteLine("Available Seats -> Sleeper: " + t.SleeperSeats + ", AC3: " + t.AC3Seats + ", AC2: " + t.AC2Seats);
            }

        }

        private static void BookTicket(ICustomerService service, int customerId)
        {
            string trainNo = InputHelper.ReadString("Train No: ");
            string classType = InputHelper.ReadString("Class Type (Sleeper/AC3/AC2): ");
            DateTime travelDate = InputHelper.ReadDate("Travel Date (yyyy-MM-dd): ");

            var seatInfo = service.GetAvailableSeatsAndPrice(trainNo, travelDate, classType);
            Console.WriteLine($"Available seats in {classType}: {seatInfo.availableSeats}, Price per seat: {seatInfo.price}");

            if (seatInfo.availableSeats <= 0)
            {
                Console.WriteLine("No seats available in the chosen class/date.");
                return;
            }

            int totalPassengers = InputHelper.ReadInt($"Number of passengers to book (max {seatInfo.availableSeats}): ");
            if (totalPassengers <= 0 || totalPassengers > seatInfo.availableSeats)
            {
                Console.WriteLine($"Invalid passenger count. Must be 1..{seatInfo.availableSeats}");
                return;
            }

            List<Passenger> passengers = new List<Passenger>();
            for (int i = 0; i < totalPassengers; i++)
            {
                Console.WriteLine($"\nPassenger {i + 1}");
                passengers.Add(new Passenger
                {
                    PassengerName = InputHelper.ReadString("Name: "),
                    Age = InputHelper.ReadInt("Age: "),
                    Gender = InputHelper.ReadString("Gender (M/F): ")
                });
            }

            string paymentMethod = InputHelper.ReadString("Payment Method (cash/card/upi): ");

            string pnr = service.BookTicket(customerId, trainNo, classType, travelDate, totalPassengers, paymentMethod, passengers);
            if (!string.IsNullOrWhiteSpace(pnr))
                Console.WriteLine($"Ticket booked successfully. PNR: {pnr}");
            else
                Console.WriteLine("Booking failed (no PNR returned).");
        }

        private static void ViewBookingByPNR(ICustomerService service)
        {
            string pnr = InputHelper.ReadString("Enter PNR: ");
            BookingDetailsDTO bookingDetails = service.ViewBookingDetailsByPNR(pnr);

            if (bookingDetails == null || bookingDetails.Booking == null)
            {
                Console.WriteLine("No booking found with this PNR.");
                return;
            }

            var b = bookingDetails.Booking;

            Console.WriteLine("\n========== Booking Details ==========");
            Console.WriteLine($"PNR: {b.PNR}");
            Console.WriteLine($"Train No: {b.TrainNo}");
            Console.WriteLine($"Class Type: {b.ClassType}");
            Console.WriteLine($"Travel Date: {b.TravelDate:yyyy-MM-dd}");
            Console.WriteLine($"Total Passengers: {b.TotalPassengers}");
            Console.WriteLine($"Total Amount: {b.TotalAmount:C}");
            Console.WriteLine($"Booking Date: {b.BookingDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"Payment Method: {b.PaymentMethod}");
            Console.WriteLine($"Status: {b.Status}");
            Console.WriteLine("======================================");

            if (bookingDetails.Passengers != null && bookingDetails.Passengers.Count > 0)
            {
                Console.WriteLine("\nPassenger List:");
                Console.WriteLine("Pid  Name  Age  Gender  Seat  Berth  Status");
                foreach (var p in bookingDetails.Passengers)
                {
                    Console.WriteLine(p.PassengerId + "  "
                        + p.PassengerName + "  "
                        + p.Age + "  "
                        + p.Gender + "  "
                        + p.SeatNo + "  "
                        + p.BerthType + "  "
                        + p.Status + " ");
                }
            

            }
       }

  
        private static void CancelPassengerTicket(ICustomerService service)
        {
            string pnr = InputHelper.ReadString("Enter PNR: ");
            BookingDetailsDTO dto = null;
            try
            {
                dto = service.ViewBookingDetailsByPNR(pnr);
            }
            catch (NotFoundException nf)
            {
                Console.WriteLine($"{nf.Message}");
                return;
            }

            if (dto?.Passengers == null || dto.Passengers.Count == 0)
            {
                Console.WriteLine("No passengers available to cancel for this PNR.");
                return;
            }

            Console.WriteLine("\nPassengers:");
            foreach (var p in dto.Passengers)
            {
                Console.WriteLine($"  {p.PassengerId} : {p.PassengerName} (Age {p.Age}, {p.Gender}) - Status: {p.Status}");
            }

            int passengerId = InputHelper.ReadInt("Enter Passenger ID to cancel: ");
            decimal refund = service.CancelPassengerTicket(pnr, passengerId);

            if (refund >= 0)
            {
                if (refund > 0)
                    Console.WriteLine($"Cancellation successful. Refund: {refund}");
                else
                    Console.WriteLine("Cancellation successful. No refund applicable.");
            }
            else
            {
                Console.WriteLine("Cancellation failed.");
            }
        }

    }
}

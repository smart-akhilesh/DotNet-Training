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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==== Railway Reservation Customer Console ====\n");
            Console.ResetColor();

            ICustomerRepository customerRepo = new CustomerRepository();
            ICustomerService customerService = new CustomerService(customerRepo);

            int loggedInCustomerId = -1;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n--- Welcome ---");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");
                Console.ResetColor();

                int firstChoice = InputHelper.ReadInt("Enter your choice: ");

                switch (firstChoice)
                {
                    case 1:
                       
                        while (true)
                        {
                            try
                            {
                                string emailId = InputHelper.ReadString("Email: ");
                                string password = InputHelper.ReadString("Password: ");

                                if (customerService.Login(emailId, password, out loggedInCustomerId))
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Login successful. CustomerId = {loggedInCustomerId}\n");
                                    Console.ResetColor();
                                    break;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid credentials. Try again.\n");
                                    Console.ResetColor();
                                }
                            }
                            catch (LoginException lex) { PrintError("Login error", lex.Message); }
                            catch (ServiceException sex) { PrintError("Service error during login", sex.Message); }
                            catch (Exception ex) { PrintError("Unexpected error", ex.Message); }
                        }
                        break;

                    case 2:
                       
                        try
                        {
                            RegisterCustomer(customerService);
                        }
                        catch (ServiceException sex) { PrintError("Service error during registration", sex.Message); }
                        catch (Exception ex) { PrintError("Unexpected error", ex.Message); }
                        break;

                    case 3:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Exiting. Goodbye!");
                        Console.ResetColor();
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Try again.");
                        Console.ResetColor();
                        break;
                }

                if (loggedInCustomerId != -1) break;
            }

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n--- Customer Menu ---");
                Console.WriteLine("1. Search Trains");
                Console.WriteLine("2. Book Ticket");
                Console.WriteLine("3. View Booking by PNR");
                Console.WriteLine("4. Cancel Passenger Ticket");
                Console.WriteLine("5. Logout");
                Console.ResetColor();

                int choice = InputHelper.ReadInt("Enter your choice: ");
                try
                {
                    switch (choice)
                    {
                        case 1: SearchTrains(customerService); break;
                        case 2: BookTicket(customerService, loggedInCustomerId); break;
                        case 3: ViewBookingByPNR(customerService); break;
                        case 4: CancelPassengerTicket(customerService); break;
                        case 5:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("Logging out. Goodbye!");
                            Console.ResetColor();
                            return;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid option. Try again.");
                            Console.ResetColor();
                            break;
                    }
                }
                catch (LoginException ex) { PrintError("Login error", ex.Message); }
                catch (NotFoundException ex) { PrintError("Not found", ex.Message); }
                catch (BookingException ex) { PrintError("Booking error", ex.Message); }
                catch (ServiceException ex) { PrintError("Service error", ex.Message); }
                catch (Exception ex) { PrintError("Unexpected error", ex.Message); }
            }
        }

        private static void PrintError(string title, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{title}: {message}");
            Console.ResetColor();
        }

        private static void RegisterCustomer(ICustomerService service)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Register New Customer ---");
            Console.ResetColor();

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
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Registration successful! You can now log in.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Registration failed.");
            }


            Console.ResetColor();
        }

        private static void SearchTrains(ICustomerService service)
        {
            string source = InputHelper.ReadString("Source: ");
            string destination = InputHelper.ReadString("Destination: ");
            DateTime travelDate = InputHelper.ReadDate("Travel Date (yyyy-MM-dd): ");

            var trains = service.ViewTrainsByRoute(source, destination, travelDate);
            if (trains == null || trains.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No trains found for this route.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nAvailable Trains:");
            Console.WriteLine("TrainNo  | TrainName  | Source  | Destination  | SleeperPrice | AC3Price | AC2Price");
            Console.ResetColor();

            foreach (var t in trains)
            {
                Console.WriteLine($"{t.TrainNo} | {t.TrainName}  |  {t.Source} |  {t.Destination} |  {t.SleeperPrice} |  {t.AC3Price} |  {t.AC2Price}");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Available Seats -> Sleeper: {t.SleeperSeats}, AC3: {t.AC3Seats}, AC2: {t.AC2Seats}");
                Console.ResetColor();
            }
        }

        private static void BookTicket(ICustomerService service, int customerId)
        {
            string trainNo = InputHelper.ReadString("Train No: ");
            string classType = InputHelper.ReadClassType("Class Type (Sleeper/AC3/AC2): ");
            DateTime travelDate = InputHelper.ReadDate("Travel Date (yyyy-MM-dd): ");

            var seatInfo = service.GetAvailableSeatsAndPrice(trainNo, travelDate, classType);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Available seats in {classType}: {seatInfo.availableSeats}, Price per seat: {seatInfo.price}");
            Console.ResetColor();

            if (seatInfo.availableSeats <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No seats available in the chosen class/date.");
                Console.ResetColor();
                return;
            }

            int totalPassengers = InputHelper.ReadInt($"Number of passengers to book (max {seatInfo.availableSeats}): ");
            if (totalPassengers <= 0 || totalPassengers > seatInfo.availableSeats)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid passenger count. Must be 1..{seatInfo.availableSeats}");
                Console.ResetColor();
                return;
            }

            List<Passenger> passengers = new List<Passenger>();
            for (int i = 0; i < totalPassengers; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nPassenger {i + 1}");
                Console.ResetColor();

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
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Ticket booked successfully. PNR: {pnr}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Booking failed (no PNR returned).");
            }

            Console.ResetColor();
        }

        private static void ViewBookingByPNR(ICustomerService service)
        {
            string pnr = InputHelper.ReadString("Enter PNR: ");
            BookingDetailsDTO bookingDetails = service.ViewBookingDetailsByPNR(pnr);

            if (bookingDetails?.Booking == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No booking found with this PNR.");
                Console.ResetColor();
                return;
            }

            var b = bookingDetails.Booking;
            Console.ForegroundColor = ConsoleColor.Cyan;
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
            Console.ResetColor();

            if (bookingDetails.Passengers != null && bookingDetails.Passengers.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nPassenger List:");
                Console.WriteLine("Pid  |  Name |  Age | Gender  | Seat |  Berth |  Status");
                Console.ResetColor();

                foreach (var p in bookingDetails.Passengers)
                {
                    Console.WriteLine($"{p.PassengerId} | {p.PassengerName} | {p.Age} |  {p.Gender} |  {p.SeatNo} | {p.BerthType} |  {p.Status}");
                }
            }
        }

        private static void CancelPassengerTicket(ICustomerService service)
        {
            string pnr = InputHelper.ReadString("Enter PNR: ");
            BookingDetailsDTO dto;
            try
            {
                dto = service.ViewBookingDetailsByPNR(pnr);
            }
            catch (NotFoundException nf)
            {
                PrintError("", nf.Message);
                return;
            }

            if (dto?.Passengers == null || dto.Passengers.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No passengers available to cancel for this PNR.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nPassengers:");
            foreach (var p in dto.Passengers)
                Console.WriteLine($"  {p.PassengerId} : {p.PassengerName} (Age {p.Age}, {p.Gender}) - Status: {p.Status}");
            Console.ResetColor();

            int passengerId = InputHelper.ReadInt("Enter Passenger ID to cancel: ");
            decimal refund = service.CancelPassengerTicket(pnr, passengerId);

            Console.ForegroundColor = ConsoleColor.Green;
            if (refund > 0) Console.WriteLine($"Cancellation successful. Refund: {refund}");
            else if (refund == 0) Console.WriteLine("Cancellation successful. No refund applicable.");
            else Console.WriteLine("Cancellation failed.");
            Console.ResetColor();
        }
    }
}

using System;
using Railway_Reservation_System_Project.Exceptions;
using Railway_Reservation_System_Project.Interfaces.Repositories;
using Railway_Reservation_System_Project.Interfaces.Services;
using Railway_Reservation_System_Project.Models;
using Railway_Reservation_System_Project.Repositories;
using Railway_Reservation_System_Project.Services;
using Railway_Reservation_System_Project.Utils;

namespace Railway_Reservation_System_Project.Client
{
    class AdminClient
    {
        public static void Run()
        {
          
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("============================================");
            Console.WriteLine("   Railway Reservation Admin Console   ");
            Console.WriteLine("============================================\n");
            Console.ResetColor();

            IAdminRepository adminRepo = new AdminRepository();
            IAdminService adminService = new AdminService(adminRepo);

            
            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("=== Admin Login ===");
                    Console.ResetColor();

                    string username = InputHelper.ReadString("Enter username: ");
                    string password = InputHelper.ReadString("Enter password: ");

                    Admin admin = new Admin { Username = username, Password = password };

                    if (adminService.Login(admin))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n Login successful.\n");
                        Console.ResetColor();
                        break;
                    }
                }
                catch (LoginException ex)
                {
                    PrintError("Login failed", ex.Message);
                    Console.ResetColor();
                }
            }

        
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n========== Admin Menu ==========");
                Console.ResetColor();
                Console.WriteLine("1. Insert New Train");
                Console.WriteLine("2. Update Train");
                Console.WriteLine("3. Delete Train");
                Console.WriteLine("4. Total Booking and Revenue");
                Console.WriteLine("5. Bookings Per Train");
                Console.WriteLine("6. Cancellation Report");
                Console.WriteLine("7. Logout");
                Console.WriteLine("==============================");

                string choice = InputHelper.ReadString("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case "1": AddTrainUI(adminService); break;
                        case "2": UpdateTrainUI(adminService); break;
                        case "3": RemoveTrainUI(adminService); break;
                        case "4": BookingRevenueReportUI(adminService); break;
                        case "5": TrainBookingReportUI(adminService); break;
                        case "6": CancellationReportUI(adminService); break;
                        case "7":
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("\n Exiting Admin Panel. Goodbye!");
                            Console.ResetColor();
                            return;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(" Invalid option. Please try again.");
                            Console.ResetColor();
                            break;
                    }
                }
                catch (TrainOperationException ex)
                {
                    PrintError("Train operation failed", ex.Message);
                }
                catch (ReportGenerationException ex)
                {
                    PrintError("Report generation failed", ex.Message);
                }
                catch (ServiceException ex)
                {
                    PrintError("Service Error", ex.Message);
                }
                catch (Exception ex)
                {
                    PrintError("Unexpected error", ex.Message);
                }
            }
        }

        private static void PrintError(string title, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($" {title}: {message}");
            Console.ResetColor();
        }

        private static void AddTrainUI(IAdminService adminService)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=== Add New Train ===");
            Console.ResetColor();

            var train = new Train
            {
                TrainNo = InputHelper.ReadInt("Train No: "),
                TrainName = InputHelper.ReadString("Train Name: "),
                Source = InputHelper.ReadString("Source: "),
                Destination = InputHelper.ReadString("Destination: "),
                SleeperSeats = InputHelper.ReadInt("Sleeper Seats: "),
                AC3Seats = InputHelper.ReadInt("AC3 Seats: "),
                AC2Seats = InputHelper.ReadInt("AC2 Seats: "),
                SleeperPrice = InputHelper.ReadDecimal("Sleeper Price: "),
                AC3Price = InputHelper.ReadDecimal("AC3 Price: "),
                AC2Price = InputHelper.ReadDecimal("AC2 Price: ")
            };

            adminService.AddTrain(train);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Train added successfully.");
            Console.ResetColor();
        }

        private static void UpdateTrainUI(IAdminService adminService)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=== Update Train ===");
            Console.ResetColor();

            var train = new Train
            {
                TrainNo = InputHelper.ReadInt("Enter Train No to Update: "),
                TrainName = InputHelper.ReadString("New Train Name: "),
                Source = InputHelper.ReadString("New Source: "),
                Destination = InputHelper.ReadString("New Destination: "),
                SleeperSeats = InputHelper.ReadInt("New Sleeper Seats: "),
                AC3Seats = InputHelper.ReadInt("New AC3 Seats: "),
                AC2Seats = InputHelper.ReadInt("New AC2 Seats: "),
                SleeperPrice = InputHelper.ReadDecimal("New Sleeper Price: "),
                AC3Price = InputHelper.ReadDecimal("New AC3 Price: "),
                AC2Price = InputHelper.ReadDecimal("New AC2 Price: ")
            };

            adminService.EditTrain(train);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Train updated successfully.");
            Console.ResetColor();
        }

        private static void RemoveTrainUI(IAdminService adminService)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=== Remove Train ===");
            Console.ResetColor();

            string trainNo = InputHelper.ReadString("Enter Train No to Delete: ");
            adminService.RemoveTrain(trainNo);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" Train deleted successfully.");
            Console.ResetColor();
        }

        private static void BookingRevenueReportUI(IAdminService adminService)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=== Booking & Revenue Report ===");
            Console.ResetColor();

            DateTime startDate = InputHelper.ReadDate("Start Date (YYYY-MM-DD): ");
            DateTime endDate = InputHelper.ReadDate("End Date (YYYY-MM-DD): ");

            var report = adminService.GenerateBookingRevenueReport(startDate, endDate);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Total Bookings: {report.TotalBookings}");
            Console.WriteLine($" Total Revenue: {report.TotalRevenue}");
            Console.ResetColor();
        }

        private static void TrainBookingReportUI(IAdminService adminService)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=== Bookings Per Train ===");
            Console.ResetColor();

            DateTime startDate = InputHelper.ReadDate("Start Date (YYYY-MM-DD): ");
            DateTime endDate = InputHelper.ReadDate("End Date (YYYY-MM-DD): ");

            var reports = adminService.GenerateTrainBookingReport(startDate, endDate);
            Console.ForegroundColor = ConsoleColor.Magenta;
            foreach (var r in reports)
            {
                Console.WriteLine($" Train Number: {r.TrainNo} | Train Name:  {r.TrainName} | Total Bookings: {r.TotalBookings}");
            }
            Console.ResetColor();
        }

        private static void CancellationReportUI(IAdminService adminService)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=== Cancellation Report ===");
            Console.ResetColor();

            DateTime startDate = InputHelper.ReadDate("Start Date (YYYY-MM-DD): ");
            DateTime endDate = InputHelper.ReadDate("End Date (YYYY-MM-DD): ");
            string trainNo = InputHelper.ReadString("Train No: ");

            var reports = adminService.GenerateCancellationReport(startDate, endDate, trainNo);
            Console.ForegroundColor = ConsoleColor.Magenta;
            foreach (var r in reports)
            {
                Console.WriteLine($" CancellationID: {r.CancellationId} | Refund: {r.RefundAmount} |  Date: {r.CancellationDate}");
            }
            Console.ResetColor();
        }
    }
}

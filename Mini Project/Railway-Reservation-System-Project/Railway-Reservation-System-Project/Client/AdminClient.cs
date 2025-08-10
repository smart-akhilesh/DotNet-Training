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
        static void Main(string[] args)
        {
            Console.WriteLine("==== Railway Reservation Admin Console ====\n");

            IAdminRepository adminRepo = new AdminRepository();
            IAdminService adminService = new AdminService(adminRepo);

            // Admin Login
            while (true)
            {
                try
                {
                    string username = InputHelper.ReadString("Enter username: ");
                    string password = InputHelper.ReadString("Enter password: ");

                    Admin admin = new Admin { Username = username, Password = password };

                    if (adminService.Login(admin))
                    {
                        Console.WriteLine(" Login successful.\n");
                        break;
                    }
                }
                catch (LoginException ex)
                {
                    Console.WriteLine($" {ex.Message}");
                }
            }

            // Menu loop
            while (true)
            {
                Console.WriteLine("\n--- Admin Menu ---");
                Console.WriteLine("1. Insert New Train");
                Console.WriteLine("2. Update Train");
                Console.WriteLine("3. Delete Train");
                Console.WriteLine("4. Total Booking and Revenue");
                Console.WriteLine("5. Bookings Per Train");
                Console.WriteLine("6. Cancellation Report");
                Console.WriteLine("7. Exit");

                string choice = InputHelper.ReadString("Enter your choice: ");

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddTrainUI(adminService);
                            break;
                        case "2":
                            UpdateTrainUI(adminService);
                            break;
                        case "3":
                            RemoveTrainUI(adminService);
                            break;
                        case "4":
                            BookingRevenueReportUI(adminService);
                            break;
                        case "5":
                            TrainBookingReportUI(adminService);
                            break;
                        case "6":
                            CancellationReportUI(adminService);
                            break;
                        case "7":
                            Console.WriteLine(" Exiting Admin Panel.");
                            return;
                        default:
                            Console.WriteLine(" Invalid option. Please try again.");
                            break;
                    }
                }
                catch (TrainOperationException ex)
                {
                    Console.WriteLine($"Train operation failed: {ex.Message}");
                }
                catch (ReportGenerationException ex)
                {
                    Console.WriteLine($"Report generation failed: {ex.Message}");
                }
                catch (ServiceException ex)
                {
                    Console.WriteLine($"Service Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }

    
        private static void AddTrainUI(IAdminService adminService)
        {
            var train = new Train
            {
                TrainNo = InputHelper.ReadString("Train No: "),
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
            Console.WriteLine("Train added successfully.");
        }

        private static void UpdateTrainUI(IAdminService adminService)
        {
            var train = new Train
            {
                TrainNo = InputHelper.ReadString("Enter Train No to Update: "),
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
            Console.WriteLine("Train updated successfully.");
        }

        private static void RemoveTrainUI(IAdminService adminService)
        {
            string trainNo = InputHelper.ReadString("Enter Train No to Delete: ");
            adminService.RemoveTrain(trainNo);
            Console.WriteLine("Train deleted successfully.");
        }

        private static void BookingRevenueReportUI(IAdminService adminService)
        {
            DateTime startDate = InputHelper.ReadDate("Start Date (yyyy-MM-dd): ");
            DateTime endDate = InputHelper.ReadDate("End Date (yyyy-MM-dd): ");

            var report = adminService.GenerateBookingRevenueReport(startDate, endDate);
            Console.WriteLine($"\n Total Bookings: {report.TotalBookings}");
            Console.WriteLine($" Total Revenue: {report.TotalRevenue:C}");
        }

        private static void TrainBookingReportUI(IAdminService adminService)
        {
            DateTime startDate = InputHelper.ReadDate("Start Date (yyyy-MM-dd): ");
            DateTime endDate = InputHelper.ReadDate("End Date (yyyy-MM-dd): ");

            var reports = adminService.GenerateTrainBookingReport(startDate, endDate);
            foreach (var r in reports)
            {
                Console.WriteLine($"{r.TrainNo} - {r.TrainName} | Bookings: {r.TotalBookings}");
            }
        }

        private static void CancellationReportUI(IAdminService adminService)
        {
            DateTime? startDate = InputHelper.ReadOptionalDate("Start Date (yyyy-MM-dd) or leave blank: ");
            DateTime? endDate = InputHelper.ReadOptionalDate("End Date (yyyy-MM-dd) or leave blank: ");
            string trainNo = InputHelper.ReadOptionalString("Train No or leave blank: ");

            var reports = adminService.GenerateCancellationReport(startDate, endDate, trainNo);
            foreach (var r in reports)
            {
                Console.WriteLine($"CancellationID: {r.CancellationId} | Refund: {r.RefundAmount:C} | Date: {r.CancellationDate}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Railway_Reservation_System_Project.Interfaces.Repositories;
using Railway_Reservation_System_Project.Interfaces.Services;
using Railway_Reservation_System_Project.Models;
using Railway_Reservation_System_Project.Exceptions;

namespace Railway_Reservation_System_Project.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public bool Login(Admin admin)
        {
            try
            {
                bool success = _adminRepository.Login(admin);

                if (!success)
                    throw new LoginException("Invalid username or password.");

                return success;
            }
            catch (Exception ex)
            {
                throw new LoginException("Admin login failed.", ex);
            }
        }

        public bool AddTrain(Train train)
        {
            try
            {
                bool success = _adminRepository.InsertTrain(train);

                if (!success)
                    throw new TrainOperationException("Failed to add train.");

                return success;
            }
            catch (Exception ex)
            {
                throw new TrainOperationException("Error occurred while adding train.", ex);
            }
        }

        public bool EditTrain(Train train)
        {
            try
            {
                bool success = _adminRepository.UpdateTrain(train);

                if (!success)
                    throw new TrainOperationException("Failed to update train.");

                return success;
            }
            catch (Exception ex)
            {
                throw new TrainOperationException("Error occurred while updating train.", ex);
            }
        }

        public bool RemoveTrain(string trainNo)
        {
            try
            {
                bool success = _adminRepository.DeleteTrain(trainNo);

                if (!success)
                    throw new TrainOperationException("Failed to remove train.");

                return success;
            }
            catch (Exception ex)
            {
                throw new TrainOperationException("Error occurred while removing train.", ex);
            }
        }

        public BookingRevenueReport GenerateBookingRevenueReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                var report = _adminRepository.GetTotalBookingsAndRevenue(startDate, endDate);

                if (report == null)
                    throw new ReportGenerationException("No booking revenue data found for the given dates.");

                return report;
            }
            catch (Exception ex)
            {
                throw new ReportGenerationException("Error generating booking revenue report.", ex);
            }
        }

        public List<TrainBookingReport> GenerateTrainBookingReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                var reports = _adminRepository.GetBookingsPerTrain(startDate, endDate);

                if (reports == null || reports.Count == 0)
                    throw new ReportGenerationException("No train booking data found for the given dates.");

                return reports;
            }
            catch (Exception ex)
            {
                throw new ReportGenerationException("Error generating train booking report.", ex);
            }
        }

        public List<CancellationReport> GenerateCancellationReport(DateTime? startDate, DateTime? endDate, string trainNo)
        {
            try
            {
                var reports = _adminRepository.GetCancellationRefunds(startDate, endDate, trainNo);

                if (reports == null || reports.Count == 0)
                    throw new ReportGenerationException("No cancellation/refund data found for the given filters.");

                return reports;
            }
            catch (Exception ex)
            {
                throw new ReportGenerationException("Error generating cancellation report.", ex);
            }
        }
    }
}

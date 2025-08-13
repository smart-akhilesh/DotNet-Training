using System;
using System.Collections.Generic;
using Railway_Reservation_System_Project.Models;

namespace Railway_Reservation_System_Project.Interfaces.Services
{
    public interface IAdminService
    {
        bool Login(Admin admin);

        bool AddTrain(Train train);

        bool EditTrain(Train train);

        decimal RemoveTrain(string trainNo);

        BookingRevenueReport GenerateBookingRevenueReport(DateTime startDate, DateTime endDate);

        List<TrainBookingReport> GenerateTrainBookingReport(DateTime startDate, DateTime endDate);

        List<CancellationReport> GenerateCancellationReport(DateTime startDate, DateTime endDate, string trainNo);
    }
}

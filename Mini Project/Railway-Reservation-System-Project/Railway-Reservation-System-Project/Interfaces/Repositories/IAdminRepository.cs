using System;
using System.Collections.Generic;
using Railway_Reservation_System_Project.Models;

namespace Railway_Reservation_System_Project.Interfaces.Repositories
{
    public interface IAdminRepository
    {
        bool Login(Admin admin);

        bool InsertTrain(Train train);
        bool UpdateTrain(Train train);
        bool DeleteTrain(string trainNo);

        BookingRevenueReport GetTotalBookingsAndRevenue(DateTime startDate, DateTime endDate);

        List<TrainBookingReport> GetBookingsPerTrain(DateTime startDate, DateTime endDate);

        List<CancellationReport> GetCancellationRefunds(DateTime startDate, DateTime endDate, string trainNo);
    }
}

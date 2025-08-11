using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Railway_Reservation_System_Project.Database;
using Railway_Reservation_System_Project.Interfaces.Repositories;
using Railway_Reservation_System_Project.Models;

namespace Railway_Reservation_System_Project.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        public bool Login(Admin admin)
        {
            var con = DbConnection.GetConnection();
            try
            {
                var cmd = new SqlCommand("VerifyAdminLogin", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Username", admin.Username);
                cmd.Parameters.AddWithValue("@Password", admin.Password);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
            finally
            {
                con.Close();
            }
        }

        public bool InsertTrain(Train train)
        {
            var con = DbConnection.GetConnection();
            try
            {
                var cmd = new SqlCommand("InsertTrain", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Trainno", train.TrainNo);
                cmd.Parameters.AddWithValue("@Trainname", train.TrainName);
                cmd.Parameters.AddWithValue("@Source", train.Source);
                cmd.Parameters.AddWithValue("@Destination", train.Destination);
                cmd.Parameters.AddWithValue("@Sleeperseats", train.SleeperSeats);
                cmd.Parameters.AddWithValue("@AC3seats", train.AC3Seats);
                cmd.Parameters.AddWithValue("@AC2seats", train.AC2Seats);
                cmd.Parameters.AddWithValue("@Sleeperprice", train.SleeperPrice);
                cmd.Parameters.AddWithValue("@AC3price", train.AC3Price);
                cmd.Parameters.AddWithValue("@AC2price", train.AC2Price);

                cmd.ExecuteNonQuery();
                return true;
            }
            finally
            {
                con.Close();
            }
        }

        public bool UpdateTrain(Train train)
        {
            var con = DbConnection.GetConnection();
            try
            {
                var cmd = new SqlCommand("UpdateTrain", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Trainno", train.TrainNo);
                cmd.Parameters.AddWithValue("@Trainname", train.TrainName);
                cmd.Parameters.AddWithValue("@Source", train.Source);
                cmd.Parameters.AddWithValue("@Destination", train.Destination);
                cmd.Parameters.AddWithValue("@Sleeperseats", train.SleeperSeats);
                cmd.Parameters.AddWithValue("@AC3seats", train.AC3Seats);
                cmd.Parameters.AddWithValue("@AC2seats", train.AC2Seats);
                cmd.Parameters.AddWithValue("@Sleeperprice", train.SleeperPrice);
                cmd.Parameters.AddWithValue("@AC3price", train.AC3Price);
                cmd.Parameters.AddWithValue("@AC2price", train.AC2Price);

                cmd.ExecuteNonQuery();
                return true;
            }
            finally
            {
                con.Close();
            }
        }

        public bool DeleteTrain(string trainNo)
        {
            var con = DbConnection.GetConnection();
            try
            {
                var cmd = new SqlCommand("DeleteTrain", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Trainno", trainNo);

                cmd.ExecuteNonQuery();
                return true;
            }
            finally
            {
                con.Close();
            }
        }

        public BookingRevenueReport GetTotalBookingsAndRevenue(DateTime startDate, DateTime endDate)
        {
            var con = DbConnection.GetConnection();
            try
            {
                var cmd = new SqlCommand("GetTotalBookingsAndRevenueByDate", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                var dr = cmd.ExecuteReader();

                var report = new BookingRevenueReport
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    TotalBookings = 0,
                    TotalRevenue = 0
                };

                if (dr.Read())
                {
                    report.TotalBookings = dr.GetInt32(dr.GetOrdinal("TotalBookings"));
                    report.TotalRevenue = dr.GetDecimal(dr.GetOrdinal("TotalRevenue"));
                }

                return report;
            }
            finally
            {
                con.Close();
            }
        }

        public List<TrainBookingReport> GetBookingsPerTrain(DateTime startDate, DateTime endDate)
        {
            var list = new List<TrainBookingReport>();
            var con = DbConnection.GetConnection();

            try
            {
                var cmd = new SqlCommand("GetBookingsPerTrainByDate", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new TrainBookingReport
                    {
                        TrainNo = dr["Trainno"].ToString(),
                        TrainName = dr["Trainname"].ToString(),
                        TotalBookings = dr.GetInt32(dr.GetOrdinal("TotalBookings"))
                    });
                }

                return list;
            }
            finally
            {
                con.Close();
            }
        }

        public List<CancellationReport> GetCancellationRefunds(DateTime startDate, DateTime endDate, string trainNo)
        {
            var list = new List<CancellationReport>();
            var con = DbConnection.GetConnection();

            try
            {
                var cmd = new SqlCommand("GetCancellationRefundsReport", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                cmd.Parameters.AddWithValue("@TrainNo", trainNo);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new CancellationReport
                    {
                        CancellationId = dr.GetInt32(dr.GetOrdinal("Cancellationid")),
                        BookingId = dr.GetInt32(dr.GetOrdinal("Bookingid")),
                        TrainNo = dr["Trainno"].ToString(),
                        PassengerId = dr.IsDBNull(dr.GetOrdinal("Passengerid")) ? 0 : dr.GetInt32(dr.GetOrdinal("Passengerid")),
                        RefundAmount = dr.GetDecimal(dr.GetOrdinal("Refundamount")),
                        CancellationDate = dr.GetDateTime(dr.GetOrdinal("Cancellationdate")),
                        RefundStatus = dr["RefundStatus"].ToString()
                    });
                }

                return list;
            }
            finally
            {
                con.Close();
            }
        }
    }
}

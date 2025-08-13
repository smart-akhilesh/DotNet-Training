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
            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("VerifyAdminLogin", con) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("@Username", admin.Username);
                cmd.Parameters.AddWithValue("@Password", admin.Password);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public bool InsertTrain(Train train)
        {
            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("InsertTrain", con) { CommandType = CommandType.StoredProcedure })
            {
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
        }

        public bool UpdateTrain(Train train)
        {
            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("UpdateTrain", con) { CommandType = CommandType.StoredProcedure })
            {
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
        }

        public decimal DeleteTrain(string trainNo)
        {
            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("DeleteTrain", con) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("@Trainno", trainNo);

           
                var refundParam = new SqlParameter("@TotalRefundAmount", SqlDbType.Decimal)
                {
                    Precision = 10,
                    Scale = 2,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(refundParam);
                cmd.ExecuteNonQuery();

                return (decimal)(refundParam.Value ?? 0);
            }
        }

        public BookingRevenueReport GetTotalBookingsAndRevenue(DateTime startDate, DateTime endDate)
        {
            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("GetTotalBookingsAndRevenueByDate", con) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                using (var dr = cmd.ExecuteReader())
                {
                    var report = new BookingRevenueReport
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        TotalBookings = 0,
                        TotalRevenue = 0
                    };

                    if (dr.Read())
                    {
                        report.TotalBookings = Convert.ToInt32(dr["TotalBookings"]);
                        report.TotalRevenue = Convert.ToDecimal(dr["TotalRevenue"]);
                    }

                    return report;
                }
            }
        }

        public List<TrainBookingReport> GetBookingsPerTrain(DateTime startDate, DateTime endDate)
        {
            var list = new List<TrainBookingReport>();

            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("GetBookingsPerTrainByDate", con) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new TrainBookingReport
                        {
                            TrainNo = dr["Trainno"].ToString(),
                            TrainName = dr["Trainname"].ToString(),
                            TotalBookings = Convert.ToInt32(dr["TotalBookings"])
                        });
                    }
                }
            }

            return list;
        }

        public List<CancellationReport> GetCancellationRefunds(DateTime startDate, DateTime endDate, string trainNo)
        {
            var list = new List<CancellationReport>();

            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("GetCancellationRefundsReport", con) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                cmd.Parameters.AddWithValue("@TrainNo", trainNo);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new CancellationReport
                        {
                            CancellationId = Convert.ToInt32(dr["Cancellationid"]),
                            BookingId = Convert.ToInt32(dr["Bookingid"]),
                            TrainNo = dr["Trainno"].ToString(),
                            PassengerId = Convert.ToInt32(dr["Passengerid"]),
                            RefundAmount = Convert.ToDecimal(dr["Refundamount"]),
                            CancellationDate = Convert.ToDateTime(dr["Cancellationdate"]),
                            RefundStatus = dr["RefundStatus"].ToString()
                        });
                    }
                }
            }

            return list;
        }
    }
}

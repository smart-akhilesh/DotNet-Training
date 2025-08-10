using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Railway_Reservation_System_Project.Database;
using Railway_Reservation_System_Project.Models;
using Railway_Reservation_System_Project.Models.DTO;

namespace Railway_Reservation_System_Project.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public int RegisterCustomer(Customer customer)
        {
            SqlConnection con = null;
            try
            {
                con = DbConnection.GetConnection();
                SqlCommand cmd = new SqlCommand("CreateCustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", customer.CustomerName);
                cmd.Parameters.AddWithValue("@Age", customer.Age);
                cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                cmd.Parameters.AddWithValue("@Email", customer.EmailId);
                cmd.Parameters.AddWithValue("@Password", customer.Password);

                SqlParameter paramCustomerId = new SqlParameter("@CustomerId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(paramCustomerId);

                cmd.ExecuteNonQuery();
                return (int)paramCustomerId.Value;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open) con.Close();
            }
        }

        public bool Login(string email, string password, out int customerId)
        {
            customerId = 0;
            SqlConnection con = null;
            try
            {
                con = DbConnection.GetConnection();
                SqlCommand cmd = new SqlCommand("logincustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);

                SqlParameter paramIsValid = new SqlParameter("@isvalid", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                SqlParameter paramCustomerId = new SqlParameter("@customerid", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(paramIsValid);
                cmd.Parameters.Add(paramCustomerId);

                cmd.ExecuteNonQuery();

                bool isValid = Convert.ToBoolean(paramIsValid.Value);
                if (isValid)
                    customerId = Convert.ToInt32(paramCustomerId.Value);

                return isValid;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open) con.Close();
            }
        }

        public List<Train> ViewTrainsByRoute(string source, string destination, DateTime travelDate)
        {
            List<Train> trains = new List<Train>();
            SqlConnection con = null;
            SqlDataReader dr = null;
            try
            {
                con = DbConnection.GetConnection();
                SqlCommand cmd = new SqlCommand("GetTrainsByRouteAndDate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Source", source);
                cmd.Parameters.AddWithValue("@Destination", destination);
                cmd.Parameters.AddWithValue("@TravelDate", travelDate);

                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    trains.Add(new Train
                    {
                        TrainNo = dr["Trainno"].ToString(),
                        TrainName = dr["Trainname"].ToString(),
                        Source = dr["Source"].ToString(),
                        Destination = dr["Destination"].ToString(),
                        SleeperPrice = dr.GetDecimal(dr.GetOrdinal("Sleeperprice")),
                        AC3Price = dr.GetDecimal(dr.GetOrdinal("AC3price")),
                        AC2Price = dr.GetDecimal(dr.GetOrdinal("AC2price")),
                        SleeperSeats = dr.GetInt32(dr.GetOrdinal("AvailableSleeperSeats")),
                        AC3Seats = dr.GetInt32(dr.GetOrdinal("AvailableAC3Seats")),
                        AC2Seats = dr.GetInt32(dr.GetOrdinal("AvailableAC2Seats"))
                    });
                }
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (con != null && con.State == ConnectionState.Open) con.Close();
            }
            return trains;
        }

        public (int availableSeats, decimal price) GetAvailableSeatsAndPrice(string trainNo, DateTime travelDate, string classType)
        {
            SqlConnection con = null;
            SqlDataReader dr = null;
            try
            {
                con = DbConnection.GetConnection();
                SqlCommand cmd = new SqlCommand("GetAvailableSeatsAndPrice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TrainNo", trainNo);
                cmd.Parameters.AddWithValue("@TravelDate", travelDate);
                cmd.Parameters.AddWithValue("@ClassType", classType);

                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return (
                        dr.GetInt32(dr.GetOrdinal("AvailableSeats")),
                        dr.GetDecimal(dr.GetOrdinal("Price"))
                    );
                }
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (con != null && con.State == ConnectionState.Open) con.Close();
            }
            return (0, 0);
        }

        public string BookTicket(int customerId, string trainNo, string classType, DateTime travelDate,
                          int totalPassengers, string paymentMethod, List<Passenger> passengers)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;

            try
            {
                con = DbConnection.GetConnection();
                cmd = new SqlCommand("CreateBookingWithPassengers", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Parameters.AddWithValue("@TrainNo", trainNo);
                cmd.Parameters.AddWithValue("@ClassType", classType);
                cmd.Parameters.AddWithValue("@TravelDate", travelDate);
                cmd.Parameters.AddWithValue("@TotalPassengers", totalPassengers);
                cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod ?? (object)DBNull.Value);

                DataTable passengerTable = new DataTable();
                passengerTable.Columns.Add("Passengername", typeof(string));
                passengerTable.Columns.Add("Age", typeof(int));
                passengerTable.Columns.Add("Gender", typeof(string));
                foreach (var p in passengers)
                {
                    passengerTable.Rows.Add(p.PassengerName, p.Age, p.Gender);
                }

                SqlParameter tvpParam = new SqlParameter("@Passengers", SqlDbType.Structured);
                tvpParam.TypeName = "dbo.PassengerType";
                tvpParam.Value = passengerTable;
                cmd.Parameters.Add(tvpParam);

                SqlParameter bookingIdParam = new SqlParameter("@BookingId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(bookingIdParam);

                SqlParameter pnrParam = new SqlParameter("@Pnr", SqlDbType.NVarChar, 10)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(pnrParam);

                cmd.ExecuteNonQuery();

                string pnr = pnrParam.Value != DBNull.Value ? pnrParam.Value.ToString() : null;
                return pnr;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (con != null) con.Close();
            }
        }

        public BookingDetailsDTO ViewBookingDetailsByPNR(string pnr)
        {
            SqlConnection con = null;
            SqlDataReader dr = null;
            try
            {
                con = DbConnection.GetConnection();
                SqlCommand cmd = new SqlCommand("ViewBookingDetailsByPNR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Pnr", pnr);

                dr = cmd.ExecuteReader();
                BookingDetailsDTO dto = null;

                if (dr.Read())
                {
                    dto = new BookingDetailsDTO
                    {
                        Booking = new Booking
                        {
                            BookingId = dr["Bookingid"] != DBNull.Value ? Convert.ToInt32(dr["Bookingid"]) : 0,
                            PNR = dr["Pnr"].ToString(),
                            CustomerId = dr["Customerid"] != DBNull.Value ? Convert.ToInt32(dr["Customerid"]) : 0,
                            TrainNo = dr["Trainno"].ToString(),
                            ClassType = dr["Classtype"].ToString(),
                            TravelDate = dr["Traveldate"] != DBNull.Value ? Convert.ToDateTime(dr["Traveldate"]) : DateTime.MinValue,
                            TotalPassengers = dr["Totalpassengers"] != DBNull.Value ? Convert.ToInt32(dr["Totalpassengers"]) : 0,
                            TotalAmount = dr["Totalamount"] != DBNull.Value ? Convert.ToDecimal(dr["Totalamount"]) : 0,
                            PaymentMethod = dr["Paymentmethod"].ToString(),
                            BookingDate = dr["Bookingdate"] != DBNull.Value ? Convert.ToDateTime(dr["Bookingdate"]) : DateTime.MinValue,
                            Status = dr["Status"].ToString()
                        },
                        Passengers = new List<Passenger>()
                    };
                }
                else
                {
                    return null;
                }

                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        dto.Passengers.Add(new Passenger
                        {
                            PassengerId = dr["Passengerid"] != DBNull.Value ? Convert.ToInt32(dr["Passengerid"]) : 0,
                            BookingId = dto.Booking.BookingId,
                            PassengerName = dr["Passengername"].ToString(),
                            Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                            Gender = dr["Gender"].ToString(),
                            Status = dr["Status"].ToString(),
                            BerthType = dr["BerthType"] == DBNull.Value ? null : dr["BerthType"].ToString(),
                            SeatNo = dr["SeatNo"] == DBNull.Value ? null : dr["SeatNo"].ToString()
                        });
                    }
                }

                return dto;
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (con != null && con.State == ConnectionState.Open) con.Close();
            }
        }

        public decimal CancelPassengerTicket(string pnr, int passengerId)
        {
            SqlConnection con = null;
            try
            {
                con = DbConnection.GetConnection();
                SqlCommand cmd = new SqlCommand("CancelPassengerTicket", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Pnr", pnr);
                cmd.Parameters.AddWithValue("@PassengerId", passengerId);

                SqlParameter refundParam = new SqlParameter("@RefundAmount", SqlDbType.Decimal)
                {
                    Precision = 10,
                    Scale = 2,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(refundParam);

                cmd.ExecuteNonQuery();
                return refundParam.Value != DBNull.Value ? Convert.ToDecimal(refundParam.Value) : 0;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open) con.Close();
            }
        }
    }
}

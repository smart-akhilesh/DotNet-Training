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
            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("CreateCustomer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", customer.CustomerName);
                cmd.Parameters.AddWithValue("@Age", customer.Age);
                cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                cmd.Parameters.AddWithValue("@Email", customer.EmailId);
                cmd.Parameters.AddWithValue("@Password", customer.Password);

                var paramCustomerId = new SqlParameter("@CustomerId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(paramCustomerId);

                cmd.ExecuteNonQuery();
                return (int)paramCustomerId.Value;
            }
        }

        public bool Login(string email, string password, out int customerId)
        {
            customerId = 0;

            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("logincustomer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);

                var paramIsValid = new SqlParameter("@isvalid", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                var paramCustomerId = new SqlParameter("@customerid", SqlDbType.Int)
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
        }

        public List<Train> ViewTrainsByRoute(string source, string destination, DateTime travelDate)
        {
            var trains = new List<Train>();

            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("GetTrainsByRouteAndDate", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Source", source);
                cmd.Parameters.AddWithValue("@Destination", destination);
                cmd.Parameters.AddWithValue("@TravelDate", travelDate);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        trains.Add(new Train
                        {
                            TrainNo = Convert.ToInt32(dr["Trainno"]),
                            TrainName = dr["Trainname"].ToString(),
                            Source = dr["Source"].ToString(),
                            Destination = dr["Destination"].ToString(),
                            SleeperPrice = Convert.ToDecimal(dr["Sleeperprice"]),
                            AC3Price = Convert.ToDecimal(dr["AC3price"]),
                            AC2Price = Convert.ToDecimal(dr["AC2price"]),
                            SleeperSeats = Convert.ToInt32(dr["AvailableSleeperSeats"]),
                            AC3Seats = Convert.ToInt32(dr["AvailableAC3Seats"]),
                            AC2Seats = Convert.ToInt32(dr["AvailableAC2Seats"])
                        });
                    }
                }
            }
            return trains;
        }

        public (int availableSeats, decimal price) GetAvailableSeatsAndPrice(string trainNo, DateTime travelDate, string classType)
        {
            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("GetAvailableSeatsAndPrice", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TrainNo", trainNo);
                cmd.Parameters.AddWithValue("@TravelDate", travelDate);
                cmd.Parameters.AddWithValue("@ClassType", classType);

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return (
                            Convert.ToInt32(dr["AvailableSeats"]),
                            Convert.ToDecimal(dr["Price"])
                        );
                    }
                }
            }
            return (0, 0);
        }

        public string BookTicket(int customerId, string trainNo, string classType, DateTime travelDate,
                                 int totalPassengers, string paymentMethod, List<Passenger> passengers)
        {
            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("CreateBookingWithPassengers", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Parameters.AddWithValue("@TrainNo", trainNo);
                cmd.Parameters.AddWithValue("@ClassType", classType);
                cmd.Parameters.AddWithValue("@TravelDate", travelDate);
                cmd.Parameters.AddWithValue("@TotalPassengers", totalPassengers);
                cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                var passengerTable = new DataTable();
                passengerTable.Columns.Add("Passengername", typeof(string));
                passengerTable.Columns.Add("Age", typeof(int));
                passengerTable.Columns.Add("Gender", typeof(string));

                foreach (var p in passengers)
                {
                    passengerTable.Rows.Add(p.PassengerName, p.Age, p.Gender);
                }

                var passengersParam = new SqlParameter("@Passengers", SqlDbType.Structured)
                {
                    TypeName = "dbo.PassengerType",
                    Value = passengerTable
                };
                cmd.Parameters.Add(passengersParam);

                var bookingIdParam = new SqlParameter("@BookingId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(bookingIdParam);

                var pnrParam = new SqlParameter("@Pnr", SqlDbType.NVarChar, 10)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(pnrParam);

                cmd.ExecuteNonQuery();

                return pnrParam.Value.ToString();
            }
        }

        public BookingDetailsDTO ViewBookingDetailsByPNR(string pnr)
        {
            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("ViewBookingDetailsByPNR", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Pnr", pnr);

                using (var dr = cmd.ExecuteReader())
                {
                    BookingDetailsDTO dto = null;

                    if (dr.Read())
                    {
                        dto = new BookingDetailsDTO
                        {
                            Booking = new Booking
                            {
                                BookingId = Convert.ToInt32(dr["Bookingid"]),
                                PNR = dr["Pnr"].ToString(),
                                CustomerId = Convert.ToInt32(dr["Customerid"]),
                                TrainNo = dr["Trainno"].ToString(),
                                ClassType = dr["Classtype"].ToString(),
                                TravelDate = Convert.ToDateTime(dr["Traveldate"]),
                                TotalPassengers = Convert.ToInt32(dr["Totalpassengers"]),
                                TotalAmount = Convert.ToDecimal(dr["Totalamount"]),
                                PaymentMethod = dr["Paymentmethod"].ToString(),
                                BookingDate = Convert.ToDateTime(dr["Bookingdate"]),
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
                                PassengerId = Convert.ToInt32(dr["Passengerid"]),
                                BookingId = dto.Booking.BookingId,
                                PassengerName = dr["Passengername"].ToString(),
                                Age = Convert.ToInt32(dr["Age"]),
                                Gender = dr["Gender"].ToString(),
                                Status = dr["Status"].ToString(),
                                BerthType = dr["BerthType"].ToString(),
                                SeatNo = dr["SeatNo"].ToString()
                            });
                        }
                    }

                    return dto;
                }
            }
        }

        public decimal CancelPassengerTicket(string pnr, int passengerId)
        {
            using (var con = DbConnection.GetConnection())
            using (var cmd = new SqlCommand("CancelPassengerTicket", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Pnr", pnr);
                cmd.Parameters.AddWithValue("@PassengerId", passengerId);

                var refundParam = new SqlParameter("@RefundAmount", SqlDbType.Decimal)
                {
                    Precision = 10,
                    Scale = 2,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(refundParam);

                cmd.ExecuteNonQuery();
                return Convert.ToDecimal(refundParam.Value);
            }
        }
    }
}

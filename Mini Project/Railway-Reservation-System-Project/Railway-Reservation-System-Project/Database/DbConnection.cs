using System;
using System.Data.SqlClient;


namespace Railway_Reservation_System_Project.Database
{
    public class DbConnection
    {
        public static SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection("Data Source=ICS-LT-223M9K3\\SQLEXPRESS; Initial Catalog=RailwayReservationSystem; user=sa; password=lv8mRF18c1278@;");
            con.Open();
            return con;
        }
    }
}

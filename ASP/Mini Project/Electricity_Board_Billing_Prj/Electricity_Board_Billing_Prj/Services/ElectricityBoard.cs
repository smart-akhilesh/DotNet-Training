using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EBDataAccessLayer;

namespace Electricity_Board_Billing_Prj
{
    public class ElectricityBoard
    {
        public void CalculateBill(ElectricityBill ebill)
        {
            int units = ebill.UnitsConsumed;
            double total = 0;

            if (units <= 100) total = 0;
            else if (units <= 300) total = (units - 100) * 1.5;
            else if (units <= 600) total = 200 * 1.5 + (units - 300) * 3.5;
            else if (units <= 1000) total = 200 * 1.5 + 300 * 3.5 + (units - 600) * 5.5;
            else total = 200 * 1.5 + 300 * 3.5 + 400 * 5.5 + (units - 1000) * 7.5;

            ebill.BillAmount = total;
        }

        public void AddBill(ElectricityBill ebill)
        {
            using (SqlConnection con = DBHandler.GetConnection())
            {
                string q = "INSERT INTO ElectricityBill (consumer_number, consumer_name, units_consumed, bill_amount) " +
                           "VALUES (@no, @name, @units, @amt)";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@no", ebill.ConsumerNumber);
                cmd.Parameters.AddWithValue("@name", ebill.ConsumerName);
                cmd.Parameters.AddWithValue("@units", ebill.UnitsConsumed);
                cmd.Parameters.AddWithValue("@amt", ebill.BillAmount);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<ElectricityBill> Generate_N_BillDetails(int num)
        {
            List<ElectricityBill> list = new List<ElectricityBill>();
            using (SqlConnection con = DBHandler.GetConnection())
            {
                
                string q = "SELECT TOP (@n) consumer_number, consumer_name, units_consumed, bill_amount " +
                           "FROM ElectricityBill ORDER BY consumer_number DESC";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@n", num);

                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    ElectricityBill b = new ElectricityBill();
                    b.ConsumerNumber = r["consumer_number"].ToString();
                    b.ConsumerName = r["consumer_name"].ToString();
                    b.UnitsConsumed = Convert.ToInt32(r["units_consumed"]);
                    b.BillAmount = Convert.ToDouble(r["bill_amount"]);
                    list.Add(b);
                }
            }
            return list;
        }
    }
}

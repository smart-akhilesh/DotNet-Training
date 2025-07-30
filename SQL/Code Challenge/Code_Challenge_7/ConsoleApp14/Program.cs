using System;
using System.Data;
using System.Data.SqlClient;

namespace CodeChallenge_7
{
    class Program
    {
        public static SqlConnection con;
        public static SqlCommand cmd;
        public static SqlDataReader dr;

        static void Main(string[] args)
        {

            InsertDataUsingProc();
            UpdateSalaryByEmpId();

            Console.Read();
        }

        static SqlConnection getConnection()
        {
            con = new SqlConnection("Data Source = ICS-LT-223M9K3\\SQLEXPRESS; Initial Catalog = InfiniteDB; user = sa; password = lv8mRF18c1278@;");
            con.Open();
            return con;
        }


        static void UpdateSalaryByEmpId()
        {
            try
            {
                con = getConnection();
                Console.WriteLine("Enter the empid to update salary:");
                int empid = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Employee details BEFORE update:");
                SqlCommand cmdBefore = new SqlCommand("select * from employee_details where empid = @empid", con);
                cmdBefore.Parameters.AddWithValue("@empid", empid);
                SqlDataReader drBefore = cmdBefore.ExecuteReader();

                if (drBefore.HasRows)
                {
                    while (drBefore.Read())
                    {
                        Console.WriteLine("Empid: " + drBefore["empid"]);
                        Console.WriteLine("Name: " + drBefore["name"]);
                        Console.WriteLine("Salary: " + drBefore["salary"]);
                        Console.WriteLine("Gender: " + drBefore["gender"]);
                    }
                }
                else
                {
                    Console.WriteLine("No employee found with empid " + empid);
                    drBefore.Close();
                    return;
                }
                drBefore.Close();

                cmd = new SqlCommand("updatesalarybyempid", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramEmpId = new SqlParameter("@empid", empid);
                paramEmpId.DbType = DbType.Int32;
                paramEmpId.Direction = ParameterDirection.Input;

                SqlParameter paramUpdatedSalary = new SqlParameter("@updatedsalary", DbType.Single);
                paramUpdatedSalary.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(paramEmpId);
                cmd.Parameters.Add(paramUpdatedSalary);

                cmd.ExecuteNonQuery();

                float updatedSalary = Convert.ToSingle(paramUpdatedSalary.Value);

                Console.WriteLine("Updated salary for empid " + empid + " is: " + updatedSalary);

                Console.WriteLine("Employee details AFTER update:");
                SqlCommand cmdAfter = new SqlCommand("select * from employee_details where empid = @empid", con);
                cmdAfter.Parameters.AddWithValue("@empid", empid);
                SqlDataReader drAfter = cmdAfter.ExecuteReader();

                if (drAfter.HasRows)
                {
                    while (drAfter.Read())
                    {
                        Console.WriteLine("Empid: " + drAfter["empid"]);
                        Console.WriteLine("Name: " + drAfter["name"]);
                        Console.WriteLine("Salary: " + drAfter["salary"]);
                        Console.WriteLine("Gender: " + drAfter["gender"]);
                    }
                }
                else
                {
                    Console.WriteLine("No employee found with empid " + empid);
                }
                drAfter.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }

        static void InsertDataUsingProc()
        {
            try
            {
                con = getConnection();

                Console.WriteLine("Enter Employee Name:");
                string name = Console.ReadLine();

                Console.WriteLine("Enter Gender (M/F):");
                string gender = Console.ReadLine();

                Console.WriteLine("Enter Given Salary:");
                float givenSalary = float.Parse(Console.ReadLine());

                cmd = new SqlCommand("insertemployeedetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter("@name", name);
                paramName.DbType = DbType.String;

                SqlParameter paramGender = new SqlParameter("@gender", gender);
                paramGender.DbType = DbType.String;

                SqlParameter paramSalary = new SqlParameter("@salary", givenSalary);
                paramSalary.DbType = DbType.Double;

                SqlParameter paramNetSalary = new SqlParameter("@Netsalary", DbType.Double);
                paramNetSalary.Direction = ParameterDirection.Output;

                SqlParameter paramEmpId = new SqlParameter();
                paramEmpId.Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.Add(paramName);
                cmd.Parameters.Add(paramSalary);
                cmd.Parameters.Add(paramGender);
                cmd.Parameters.Add(paramNetSalary);
                cmd.Parameters.Add(paramEmpId);

                cmd.ExecuteReader();

                Console.WriteLine("Record inserted successfully!");
                Console.WriteLine("Generated EmpId: " + paramEmpId.Value);
                Console.WriteLine("Calculated Salary (after 10% deduction): " + paramNetSalary.Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
        }
    }
}



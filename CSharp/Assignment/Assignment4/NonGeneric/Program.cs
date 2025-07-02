using System;
using System.Collections.Generic;

namespace NonGeneric
{
    class Employee
    {
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public string Department { get; set; }
        public double Salary { get; set; }
    }

    class EmployeeManager
    {
        private List<Employee> emplist = new List<Employee>();

        public void AddEmployee(Employee employee)
        {
            emplist.Add(employee);
        }

        public List<Employee> DisplayEmployees()
        {
            return emplist;
        }

        public List<Employee> SearchEmpDetail(int id)
        {
            return emplist.FindAll(e => e.EmpID == id);
        }

        public void UpdateEmpDetail(int id)
        {
            var employee = emplist.Find(e => e.EmpID == id);
            if (employee != null)
            {
                try
                {
                    Console.Write("Enter New Name: ");
                    employee.EmpName = Console.ReadLine();

                    Console.Write("Enter New Department: ");
                    employee.Department = Console.ReadLine();

                    Console.Write("Enter New Salary: ");
                    employee.Salary = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Update Successful.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid salary input. Update failed.");
                }
            }
            else
            {
                Console.WriteLine("Employee Not Found.");
            }
        }

        public void DeleteEmployee(int id)
        {
            int removedCount = emplist.RemoveAll(e => e.EmpID == id);
            Console.WriteLine(removedCount > 0 ? "Delete Successful." : "Employee Not Found.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            EmployeeManager manager = new EmployeeManager();
            int choice = 0;

            do
            {
                Console.WriteLine("\n++++++++ Employee Management Menu ++++++++");
                Console.WriteLine("1. Add New Employee");
                Console.WriteLine("2. View All Employees");
                Console.WriteLine("3. Search Employee by ID");
                Console.WriteLine("4. Update Employee Details");
                Console.WriteLine("5. Delete Employee");
                Console.WriteLine("6. Exit");
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++");
                Console.Write("Enter your choice: ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid number.");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            Employee emp = new Employee();

                            Console.Write("Enter Employee ID: ");
                            emp.EmpID = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter Employee Name: ");
                            emp.EmpName = Console.ReadLine();

                            Console.Write("Enter Department: ");
                            emp.Department = Console.ReadLine();

                            Console.Write("Enter Salary: ");
                            emp.Salary = Convert.ToDouble(Console.ReadLine());

                            manager.AddEmployee(emp);
                            Console.WriteLine("Employee Added Successfully.");
                            break;

                        case 2:
                            List<Employee> allEmployees = manager.DisplayEmployees();
                            if (allEmployees.Count == 0)
                            {
                                Console.WriteLine("No employees found.");
                            }
                            else
                            {
                                Console.WriteLine("\n--- Employee List ---");
                                foreach (var e in allEmployees)
                                {
                                    Console.WriteLine($"ID: {e.EmpID}, Name: {e.EmpName}, Dept: {e.Department}, Salary: {e.Salary}");
                                }
                            }
                            break;

                        case 3:
                            Console.Write("Enter Employee ID to Search: ");
                            int searchId = Convert.ToInt32(Console.ReadLine());
                            List<Employee> found = manager.SearchEmpDetail(searchId);

                            if (found.Count == 0)
                            {
                                Console.WriteLine("Employee not found.");
                            }
                            else
                            {
                                foreach (var e in found)
                                {
                                    Console.WriteLine($"ID: {e.EmpID}, Name: {e.EmpName}, Dept: {e.Department}, Salary: {e.Salary}");
                                }
                            }
                            break;

                        case 4:
                            Console.Write("Enter Employee ID to Update: ");
                            int updateId = Convert.ToInt32(Console.ReadLine());
                            manager.UpdateEmpDetail(updateId);
                            break;

                        case 5:
                            Console.Write("Enter Employee ID to Delete: ");
                            int deleteId = Convert.ToInt32(Console.ReadLine());
                            manager.DeleteEmployee(deleteId);
                            break;

                        case 6:
                            Console.WriteLine("Exiting program.");
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please enter a number from 1 to 6.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format. Please enter correct values.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something went wrong: " + ex.Message);
                }

            } while (choice != 6);

            Console.Read();
        }
    }
}

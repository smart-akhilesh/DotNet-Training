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
            bool found = false;
            foreach (var item in emplist)
            {
                if (item.EmpID == id)
                {
                    Console.WriteLine("Enter New Name:");
                    item.EmpName = Console.ReadLine();

                    Console.WriteLine("Enter New Department:");
                    item.Department = Console.ReadLine();

                    Console.WriteLine("Enter New Salary:");
                    item.Salary = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Update Successful.");
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine("Employee Not Found.");
            }
        }

        public void DeleteEmployee(int id)
        {
            int removedCount = emplist.RemoveAll(e => e.EmpID == id);
            if (removedCount > 0)
                Console.WriteLine("Delete Successful.");
            else
                Console.WriteLine("Employee Not Found.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            EmployeeManager manager = new EmployeeManager();
            int choice;

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

                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Employee newEmp = new Employee();

                        Console.Write("Enter Employee ID: ");
                        newEmp.EmpID = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Enter Employee Name: ");
                        newEmp.EmpName = Console.ReadLine();

                        Console.Write("Enter Department: ");
                        newEmp.Department = Console.ReadLine();

                        Console.Write("Enter Salary: ");
                        newEmp.Salary = Convert.ToDouble(Console.ReadLine());

                        manager.AddEmployee(newEmp);
                        Console.WriteLine("Employee Added Successfully.");
                        break;

                    case 2:
                        var allEmployees = manager.DisplayEmployees();
                        if (allEmployees.Count == 0)
                        {
                            Console.WriteLine("No employees to display.");
                        }
                        else
                        {
                            foreach (var emp in allEmployees)
                            {
                                Console.WriteLine($"ID: {emp.EmpID}, Name: {emp.EmpName}, Department: {emp.Department}, Salary: {emp.Salary}");
                            }
                        }
                        break;

                    case 3:
                        Console.Write("Enter Employee ID to search: ");
                        int searchId = Convert.ToInt32(Console.ReadLine());
                        var foundEmployees = manager.SearchEmpDetail(searchId);
                        if (foundEmployees.Count > 0)
                        {
                            foreach (var emp in foundEmployees)
                            {
                                Console.WriteLine($"ID: {emp.EmpID}, Name: {emp.EmpName}, Department: {emp.Department}, Salary: {emp.Salary}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Employee not found.");
                        }
                        break;

                    case 4:
                        Console.Write("Enter Employee ID to update: ");
                        int updateId = Convert.ToInt32(Console.ReadLine());
                        manager.UpdateEmpDetail(updateId);
                        break;

                    case 5:
                        Console.Write("Enter Employee ID to delete: ");
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

            } while (choice != 6);
        }
    }
}




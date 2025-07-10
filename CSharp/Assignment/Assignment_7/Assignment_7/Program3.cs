using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment_7
{
    class Employee
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpCity { get; set; }
        public double EmpSalary { get; set; }
    }

    class Program3
    {
        static void Main()
        {
            var employees = new List<Employee>();

            while (true)
            {
                Console.WriteLine("\n--- Employee Management ---");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Display All Employees");
                Console.WriteLine("3. Employees with Salary > 45000");
                Console.WriteLine("4. Employees from Bangalore");
                Console.WriteLine("5. Sort by Name (Ascending)");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        Console.Write("Employee ID: ");
                        int empId = int.Parse(Console.ReadLine());

                        Console.Write("Name: ");
                        string empName = Console.ReadLine();

                        Console.Write("City: ");
                        string empCity = Console.ReadLine();

                        Console.Write("Salary: ");
                        double empSalary = double.Parse(Console.ReadLine());

                        employees.Add(new Employee
                        {
                            EmpId = empId,
                            EmpName = empName,
                            EmpCity = empCity,
                            EmpSalary = empSalary
                        });

                        Console.WriteLine("Employee added successfully.");
                        break;

                    case 2:
                        DisplayEmployees(employees);
                        break;

                    case 3:
                        DisplayEmployees(employees.Where(e => e.EmpSalary > 45000));
                        break;

                    case 4:
                        DisplayEmployees(employees.Where(e =>
                            e.EmpCity.Equals("Bangalore", StringComparison.OrdinalIgnoreCase)));
                        break;

                    case 5:
                        DisplayEmployees(employees.OrderBy(e => e.EmpName));
                        break;

                    case 6:
                        Console.WriteLine("Exiting program.");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 6.");
                        break;
                }
            }
        }

        static void DisplayEmployees(IEnumerable<Employee> employees)
        {
            if (!employees.Any())
            {
                Console.WriteLine("No employees found.");
                return;
            }

            foreach (var emp in employees)
            {
                Console.WriteLine($"ID: {emp.EmpId}, Name: {emp.EmpName}, City: {emp.EmpCity}, Salary: {emp.EmpSalary}");
            }
        }
    }
}

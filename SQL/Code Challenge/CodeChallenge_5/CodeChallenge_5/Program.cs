using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChallenge_5
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DOJ { get; set; }
        public string City { get; set; }
    }

    public class Program
    {
        public static void Main()
        {
            var empList = new List<Employee>
        {
            new Employee{ EmployeeID=1001, FirstName="Malcolm", LastName="Daruwalla", Title="Manager", DOB=DateTime.ParseExact("16-11-1984", "dd-MM-yyyy", null), DOJ=DateTime.ParseExact("08-06-2011", "dd-MM-yyyy", null), City="Mumbai" },
            new Employee{ EmployeeID=1002, FirstName="Asdin", LastName="Dhalla", Title="AsstManager", DOB=DateTime.ParseExact("20-08-1994", "dd-MM-yyyy", null), DOJ=DateTime.ParseExact("07-07-2012", "dd-MM-yyyy", null), City="Mumbai" },
            new Employee{ EmployeeID=1003, FirstName="Madhavi", LastName="Oza", Title="Consultant", DOB=DateTime.ParseExact("14-11-1987", "dd-MM-yyyy", null), DOJ=DateTime.ParseExact("12-04-2015", "dd-MM-yyyy", null), City="Pune" },
            new Employee{ EmployeeID=1004, FirstName="Saba", LastName="Shaikh", Title="SE", DOB=DateTime.ParseExact("03-06-1990", "dd-MM-yyyy", null), DOJ=DateTime.ParseExact("02-02-2016", "dd-MM-yyyy", null), City="Pune" },
            new Employee{ EmployeeID=1005, FirstName="Nazia", LastName="Shaikh", Title="SE", DOB=DateTime.ParseExact("08-03-1991", "dd-MM-yyyy", null), DOJ=DateTime.ParseExact("02-02-2016", "dd-MM-yyyy", null), City="Mumbai" },
            new Employee{ EmployeeID=1006, FirstName="Amit", LastName="Pathak", Title="Consultant", DOB=DateTime.ParseExact("07-11-1989", "dd-MM-yyyy", null), DOJ=DateTime.ParseExact("08-08-2014", "dd-MM-yyyy", null), City="Chennai" },
            new Employee{ EmployeeID=1007, FirstName="Vijay", LastName="Natrajan", Title="Consultant", DOB=DateTime.ParseExact("02-12-1989", "dd-MM-yyyy", null), DOJ=DateTime.ParseExact("01-06-2015", "dd-MM-yyyy", null), City="Mumbai" },
            new Employee{ EmployeeID=1008, FirstName="Rahul", LastName="Dubey", Title="Associate", DOB=DateTime.ParseExact("11-11-1993", "dd-MM-yyyy", null), DOJ=DateTime.ParseExact("06-11-2014", "dd-MM-yyyy", null), City="Chennai" },
            new Employee{ EmployeeID=1009, FirstName="Suresh", LastName="Mistry", Title="Associate", DOB=DateTime.ParseExact("12-08-1992", "dd-MM-yyyy", null), DOJ=DateTime.ParseExact("03-12-2014", "dd-MM-yyyy", null), City="Chennai" },
            new Employee{ EmployeeID=1010, FirstName="Sumit", LastName="Shah", Title="Manager", DOB=DateTime.ParseExact("12-04-1991", "dd-MM-yyyy", null), DOJ=DateTime.ParseExact("02-01-2016", "dd-MM-yyyy", null), City="Mumbai" }
        };

            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Display all employees");
                Console.WriteLine("2. Display employees NOT in Mumbai");
                Console.WriteLine("3. Display employees with Title 'AsstManager'");
                Console.WriteLine("4. Display employees whose Last Name starts with 'S'");
                Console.WriteLine("5. Exit");

                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("\na. All Employees:");
                        foreach (var emp in empList)
                            PrintEmployee(emp);
                        break;

                    case "2":
                        Console.WriteLine("\nb. Employees NOT in Mumbai:");
                        var notMumbaiEmployees = empList.Where(e => !e.City.Equals("Mumbai", StringComparison.OrdinalIgnoreCase));
                        foreach (var emp in notMumbaiEmployees)
                            PrintEmployee(emp);
                        break;

                    case "3":
                        Console.WriteLine("\nc. Employees with Title 'AsstManager':");
                        var asstManagers = empList.Where(e => e.Title.Equals("AsstManager", StringComparison.OrdinalIgnoreCase));
                        foreach (var emp in asstManagers)
                            PrintEmployee(emp);
                        break;

                    case "4":
                        Console.WriteLine("\nd. Employees whose Last Name starts with 'S':");
                        var lastNameStartsWithS = empList.Where(e => e.LastName.StartsWith("S", StringComparison.OrdinalIgnoreCase));
                        foreach (var emp in lastNameStartsWithS)
                            PrintEmployee(emp);
                        break;

                    case "5":
                        Console.WriteLine("Exiting the program");
                        return;

                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
                Console.Read();
            }
        
        }
       

        private static void PrintEmployee(Employee emp)
        {
            Console.WriteLine($"{emp.EmployeeID}, {emp.FirstName} {emp.LastName}, {emp.Title}, DOB: {emp.DOB}, DOJ: {emp.DOJ}, City: {emp.City}");
        }
    }

}
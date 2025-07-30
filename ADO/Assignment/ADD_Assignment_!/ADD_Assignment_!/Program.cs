using System;
using System.Collections.Generic;
using System.Linq;

namespace ADD_Assignment_1
{
    class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DOJ { get; set; }
        public string City { get; set; }
    }

    class EmployeeOperations
    {
        public void Display(IEnumerable<Employee> employees)
        {
            foreach (var e in employees)
            {
                Console.WriteLine($"Employee ID : {e.EmployeeID}, Employee Full Name : {e.FirstName} {e.LastName}, Employee Title : {e.Title}, Employee DOB : {e.DOB.ToShortDateString()}, Employee DOJ : {e.DOJ.ToShortDateString()}, Employee City: {e.City}");
            }
        }

        public void ShowMenu(List<Employee> empList)
        {
            int choice;
            do
            {
                Console.WriteLine("\n************* Employee Menu *************************");
                Console.WriteLine("1. Employees who joined before 01/01/2015");
                Console.WriteLine("2. Employees born after 01/01/1990");
                Console.WriteLine("3. Employees with designation Consultant or Associate");
                Console.WriteLine("4. Total number of employees");
                Console.WriteLine("5. Total number of employees in Chennai");
                Console.WriteLine("6. Highest Employee ID");
                Console.WriteLine("7. Employees who joined after 01/01/2015");
                Console.WriteLine("8. Employees whose designation is not Associate");
                Console.WriteLine("9. Number of employees based on City");
                Console.WriteLine("10. Number of employees based on City and Title");
                Console.WriteLine("11. Youngest Employee");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                string input = Console.ReadLine();

                if (!int.TryParse(input, out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value.");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            var joinedBefore2015 = empList.Where(e => e.DOJ < DateTime.Parse("01/01/2015"));
                            Display(joinedBefore2015);
                            break;

                        case 2:
                            var dobAfter1990 = empList.Where(e => e.DOB > DateTime.Parse("01/01/1990"));
                            Display(dobAfter1990);
                            break;

                        case 3:
                            var consOrAssoc = empList.Where(e => e.Title == "Consultant" || e.Title == "Associate");
                            Display(consOrAssoc);
                            break;

                        case 4:
                            Console.WriteLine($"Total number of employees: {empList.Count}");
                            break;

                        case 5:
                            Console.WriteLine($"Employees in Chennai: {empList.Count(e => e.City == "Chennai")}");
                            break;

                        case 6:
                            Console.WriteLine($"Highest Employee ID: {empList.Max(e => e.EmployeeID)}");
                            break;

                        case 7:
                            Console.WriteLine($"Employees who joined after 01/01/2015: {empList.Count(e => e.DOJ > DateTime.Parse("01/01/2015"))}");
                            break;

                        case 8:
                            Console.WriteLine($"Employees not Associate: {empList.Count(e => e.Title != "Associate")}");
                            break;

                        case 9:
                            var cityGroup = empList.GroupBy(e => e.City);
                            foreach (var group in cityGroup)
                                Console.WriteLine($"{group.Key}: {group.Count()}");
                            break;

                        case 10:
                            var cityTitleGroup = empList.GroupBy(e => new { e.City, e.Title });
                            foreach (var group in cityTitleGroup)
                                Console.WriteLine($"{group.Key.City} - {group.Key.Title}: {group.Count()}");
                            break;

                        case 11:
                            var youngestDOB = empList.Max(e => e.DOB);
                            var youngestEmp = empList.Where(e => e.DOB == youngestDOB);
                            Display(youngestEmp);
                            break;

                        case 0:
                            Console.WriteLine("Exiting the program...");
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please enter a valid option.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while processing your request: {ex.Message}");
                }

            } while (choice != 0);
        }
    }

    class Program
    {
        static void Main()
        {
            var empList = new List<Employee>
            {
                new Employee{EmployeeID=1001, FirstName="Malcolm", LastName="Daruwalla", Title="Manager", DOB=DateTime.Parse("11/16/1984"), DOJ=DateTime.Parse("06/08/2011"), City="Mumbai"},
                new Employee{EmployeeID=1002, FirstName="Asdin", LastName="Dhalla", Title="AsstManager", DOB=DateTime.Parse("08/20/1984"), DOJ=DateTime.Parse("07/07/2012"), City="Mumbai"},
                new Employee{EmployeeID=1003, FirstName="Madhavi", LastName="Oza", Title="Consultant", DOB=DateTime.Parse("11/14/1987"), DOJ=DateTime.Parse("04/12/2015"), City="Pune"},
                new Employee{EmployeeID=1004, FirstName="Saba", LastName="Shaikh", Title="SE", DOB=DateTime.Parse("06/03/1990"), DOJ=DateTime.Parse("02/02/2016"), City="Pune"},
                new Employee{EmployeeID=1005, FirstName="Nazia", LastName="Shaikh", Title="SE", DOB=DateTime.Parse("03/08/1991"), DOJ=DateTime.Parse("02/02/2016"), City="Mumbai"},
                new Employee{EmployeeID=1006, FirstName="Amit", LastName="Pathak", Title="Consultant", DOB=DateTime.Parse("11/07/1989"), DOJ=DateTime.Parse("08/08/2014"), City="Chennai"},
                new Employee{EmployeeID=1007, FirstName="Vijay", LastName="Natrajan", Title="Consultant", DOB=DateTime.Parse("12/02/1989"), DOJ=DateTime.Parse("06/01/2015"), City="Mumbai"},
                new Employee{EmployeeID=1008, FirstName="Rahul", LastName="Dubey", Title="Associate", DOB=DateTime.Parse("11/11/1993"), DOJ=DateTime.Parse("11/06/2014"), City="Chennai"},
                new Employee{EmployeeID=1009, FirstName="Suresh", LastName="Mistry", Title="Associate", DOB=DateTime.Parse("08/12/1992"), DOJ=DateTime.Parse("12/03/2014"), City="Chennai"},
                new Employee{EmployeeID=1010, FirstName="Sumit", LastName="Shah", Title="Manager", DOB=DateTime.Parse("04/12/1991"), DOJ=DateTime.Parse("01/02/2016"), City="Pune"}
            };

            EmployeeOperations empOps = new EmployeeOperations();

            try
            {
                empOps.ShowMenu(empList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}

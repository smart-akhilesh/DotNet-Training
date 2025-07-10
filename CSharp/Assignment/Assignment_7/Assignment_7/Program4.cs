using System;
using TravelLibrary;

namespace Assignment_7
{
    class Program4
    {
        const double TotalFare = 500.00;

        static void Main()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Age: ");
            int age = int.Parse(Console.ReadLine());

            var calculator = new ConcessionCalculator();
            calculator.CalculateConcession(name, age, TotalFare);
        }
    }
}

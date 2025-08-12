using System;
using Railway_Reservation_System_Project.Client;
using Railway_Reservation_System_Project.Utils;

namespace Railway_Reservation_System_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Welcome to the Railway Reservation System");
            Console.ResetColor();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nPlease select your role:");
                Console.WriteLine("1. Admin");
                Console.WriteLine("2. Customer");
                Console.WriteLine("3. Exit");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Gray;
                string choice = InputHelper.ReadString("Enter your choice: ");
                Console.ResetColor();

                switch (choice)
                {
                    case "1":
                        Console.ForegroundColor = ConsoleColor.Green;
                        AdminClient.Run(); 
                        Console.ResetColor();
                        break;

                    case "2":
                        Console.ForegroundColor = ConsoleColor.Green;
                        CustomerClient.Run(); 
                        Console.ResetColor();
                        break;

                    case "3":
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\nThank you for using the Railway Reservation System. Goodbye!");
                        Console.Read();
                        Console.ResetColor();
                        return; 

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. Please enter 1, 2, or 3.");
                        Console.ResetColor();
                        break;
                }
            }
           
        }
    }
}

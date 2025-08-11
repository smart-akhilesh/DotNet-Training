using System;

namespace Railway_Reservation_System_Project.Utils
{
    public static class InputHelper
    {

        private static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static string ReadClassType(string prompt)
        {
            string[] validClasses = { "Sleeper", "AC3", "AC2" };

            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(input))
                {
         
                    foreach (var classType in validClasses)
                    {
                        if (classType.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                            return classType; 
                    }
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid class type. Please enter Sleeper, AC3, or AC2.");
                Console.ResetColor();
            }
        }

        public static string ReadString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                PrintError("Input cannot be empty. Please try again.");
            }
        }

        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value >= 0)
                    return value;

                PrintError("Invalid input. Please enter a valid number.");
            }
        }

        public static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal value) && value >= 0)
                    return value;

                PrintError("Invalid input. Please enter a valid decimal.");
            }
        }

        public static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null,
                    System.Globalization.DateTimeStyles.None, out DateTime date))
                    return date;

                PrintError("Invalid date format. Please enter in YYYY-MM-DD format.");
            }
        }
    }
}

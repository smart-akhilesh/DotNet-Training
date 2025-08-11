using System;

namespace Railway_Reservation_System_Project.Utils

{
    public static class InputHelper
    {
        public static string ReadString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }

        public static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal value))
                    return value;
                Console.WriteLine("Invalid input. Please enter a valid decimal.");
            }
        }

        public static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                    return date;
                Console.WriteLine("Invalid date format. Please enter in yyyy-MM-dd format.");
            }
        }

    }
}

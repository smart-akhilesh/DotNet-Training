using System;

namespace FirstAssignment
{
    class Program2
    {
        public static void Main()
        {
            Console.WriteLine("Program to Check if the number is positive or negative");
            Console.WriteLine();
            Console.Write("Enter the number : ");
            int number = Convert.ToInt32(Console.ReadLine());
            if (number < 0)
            {
                Console.WriteLine("Number is negative");
            }
            else
            {
                Console.WriteLine("Number is positive");
            }
            Console.Read();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment_7
{
    class Program
    {
        static void Main()
        {
            int totalNo = 0;
            while (true)
            {
                Console.Write("How many numbers do you want to input? ");
                try
                {
                    totalNo = int.Parse(Console.ReadLine());
                    if (totalNo <= 0)
                    {
                        Console.WriteLine("Please enter a positive number.");
                        continue;
                    }
                    break; // valid input, exit loop
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Number too large. Please enter a smaller integer.");
                }
            }

            var numbers = new List<int>();

            Console.WriteLine("Enter the numbers:");
            for (int i = 0; i < totalNo; i++)
            {
                while (true)
                {
                    try
                    {
                        numbers.Add(int.Parse(Console.ReadLine()));
                        break; 
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer.");
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("Number too large. Please enter a smaller integer.");
                    }
                }
            }

            var result = numbers
                .Select(n => new { Number = n, Square = n * n })
                .Where(x => x.Square > 20);

            foreach (var item in result)
                Console.WriteLine($"{item.Number} - {item.Square}");
        }
    }
}

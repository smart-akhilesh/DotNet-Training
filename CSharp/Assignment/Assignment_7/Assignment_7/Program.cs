using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment_7
{
    class Program
    {
        static void Main()
        {
            Console.Write("How many numbers do you want to input? ");
            int totalNo = int.Parse(Console.ReadLine());

            var numbers = new List<int>();

            Console.WriteLine("Enter the numbers:");
            for (int i = 0; i < totalNo; i++)
                numbers.Add(int.Parse(Console.ReadLine()));

            var result = numbers
                .Select(n => new { Number = n, Square = n * n })
                .Where(x => x.Square > 20);

            foreach (var item in result)
                Console.WriteLine($"{item.Number} - {item.Square}");
        }
    }
}

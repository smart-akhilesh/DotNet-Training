using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment_7
{
    class Program2
    {
        static void Main()
        {
            Console.Write("How many words do you want to input? ");
            int totalNo = int.Parse(Console.ReadLine());

            var words = new List<string>();

            Console.WriteLine("Enter the words:");
            for (int i = 0; i < totalNo; i++)
                words.Add(Console.ReadLine());

            var result = words
                .Where(word => word.StartsWith("a", StringComparison.OrdinalIgnoreCase)
                            && word.EndsWith("m", StringComparison.OrdinalIgnoreCase));

            Console.WriteLine("Matching words:");
            foreach (var word in result)
                Console.WriteLine(word);
        }
    }
}

/*
1.Write a C# Sharp program to remove the character at a given position in the string. The given position will be in the range 0..(string length -1) inclusive.
 
Sample Input:
"Python", 1
"Python", 0
"Python", 4
Expected Output:
Pthon
ython
Pythn
*/
using System;

namespace CodeChallengeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the string: ");
            string inputString = Console.ReadLine();

            Console.Write("Enter the index: ");
            int index = Convert.ToInt32(Console.ReadLine());

            if (index < 0 || index >= inputString.Length)
            {
                Console.WriteLine("Invalid index.");
                return;
            }

            string result = inputString.Substring(0, index) + inputString.Substring(index + 1);
            Console.WriteLine("String after removing the character at given index: " + result);

            Console.Read();
        }
    }
}



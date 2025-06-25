/*
2.Write a C# Sharp program to exchange the first and last characters in a given string and return the new string.
 
Sample Input:
"abcd"
"a"
"xy"
Expected Output:
 
dbca
a
yx
*/
using System;

namespace CodeChallengeFirst
{
    class Program2
    {
        public static void Main()
        {
            Console.Write("Enter the string: ");
            string input = Console.ReadLine();
            char firstChar = input[0];
            char lastChar = input[input.Length - 1];
            string middle = input.Substring(1, input.Length - 2);
            string result = lastChar + middle + firstChar;
            Console.WriteLine(result);
            Console.Read();
        }
    }
}

using System;
using System.Linq;

namespace Assignment2
{
    class Prgram8
    {
        public static void Main()
        {
            Console.WriteLine("Program to display the reverse of the word");
            Console.WriteLine();
            Console.Write("Enter the word : ");
            string word = Console.ReadLine();
            string reversedWord = new string(word.Reverse().ToArray());
            Console.WriteLine("Reversed of the word is : {0}", reversedWord);
            Console.Read();
        }
    }
}

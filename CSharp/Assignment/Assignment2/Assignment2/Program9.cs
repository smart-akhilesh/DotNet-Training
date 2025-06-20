using System;

namespace Assignment2
{
    class Program9
    {
        public static void Main()
        {
            Console.WriteLine("Program to check two words are same or not");
            Console.WriteLine();
            Console.Write("Enter first word : ");
            string word1 = Console.ReadLine();
            Console.Write("Enter second word : ");
            string word2 = Console.ReadLine();
            CheckWordEqual(word1, word2);
            Console.Read();
        }

        public static void CheckWordEqual(string word1, string word2)
        {
            bool checkEqual = word1.Equals(word2);
            if (checkEqual == true)
            {
                Console.WriteLine("Two words are same");
            }
            else
            {
                Console.WriteLine("Two words are not same");
            }
        }
    }
}

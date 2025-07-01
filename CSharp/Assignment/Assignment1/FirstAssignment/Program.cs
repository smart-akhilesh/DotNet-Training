using System;

namespace FirstAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            numberChecker();
        }

        static void numberChecker()
        {
            Console.WriteLine("Program to check first and second is equal or not");
            Console.WriteLine();
            Console.Write("Enter first number : ");
            int fnum = Convert.ToInt32(Console.ReadLine()); 
            Console.Write("Enter second number : ");
            int snum = Convert.ToInt32(Console.ReadLine()); 

            if (fnum == snum)
            {
                Console.WriteLine("First and Second numbers are equal");
            }
            else
            {
                Console.WriteLine("First and Second numbers are not equal");
            }

            Console.Read(); 
        }
    }
}

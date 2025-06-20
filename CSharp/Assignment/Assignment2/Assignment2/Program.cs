using System;

namespace Assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program to swap two number");
            Console.WriteLine();
            int a = 2, b = 3;
            Console.WriteLine("Before Swapping : a = {0}, b = {1}", a, b);
            swap(a, b);
            Console.Read();
        }
        static void swap(int a, int b)
        {
            int temp = a;
            a = b;
            b = temp;
            Console.WriteLine("After Swapping : a = {0}, b = {1}", a, b);
        }
    }
}


using System;


namespace FirstAssignment
{
    class Program4
    {
        public static void Main()
        {
            Console.WriteLine("Program to print the table of any number");
            Console.WriteLine("Enter the number");
            int number = Convert.ToInt32(Console.ReadLine());
            for(int i=0; i<=10; i++)
            { 
               Console.WriteLine($"{number} * {i} = {number * i}");
            }
            Console.Read();
        }
    }
}

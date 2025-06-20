using System;


namespace Assignment2
{
    class Program3
    {
        public enum Days { Monday = 1, Tuesday = 2, Wednesday = 3 , Thursday = 4 , Friday = 5, Saturday = 6, Sunday =7 };
        public static void Main()
        {
            Console.WriteLine("Program to print the day");
            Console.Write("Enter the value : ");
            int value = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Day is : {0}",Enum.GetName(typeof(Days), value));
            Console.Read();
        }
    }
}

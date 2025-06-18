using System;


namespace FirstAssignment
{
    class Program5
    {
        public static void Main()
        {
            Console.WriteLine("Program to return the sum or triple of sum of two no.");
            Console.WriteLine("Enter the first no.");
            int fnum = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the second no.");
            int secnum = Convert.ToInt32(Console.ReadLine());
            if(fnum == secnum)
            {
                Console.WriteLine($"Triple is {(fnum + secnum) * 3}");
            }
            else
            {
                Console.WriteLine($"Sum is {fnum + secnum}");
            }
            Console.Read();
        }
    }
}

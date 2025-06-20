using System;


namespace Assignment2
{
    class Program2
    {
        public static void Main()
        {
            Console.WriteLine("Program to print four times no.");
            Console.WriteLine();
            Console.Write("Enter the number : ");
            int num = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == 1 || i == 3)
                    {
                        Console.Write("{0}", num);
                    }
                    else
                    {
                        Console.Write("{0}", num);
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            Console.Read();
        }
    }
}

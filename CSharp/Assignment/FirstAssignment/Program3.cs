using System;

namespace FirstAssignment
{
    class Program3
    {
        public static void Main()
        {
            Console.WriteLine("Program to perform different operation");
            Console.WriteLine("Enter first number:");
            int firstnum = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter operation (+, -, *, /):");
            char operation = Convert.ToChar(Console.ReadLine());

            Console.WriteLine("Enter second number:");
            int secondnum = Convert.ToInt32(Console.ReadLine());

            switch (operation)
            {
                case '+':
                    Console.WriteLine($"{firstnum} + {secondnum} = {firstnum + secondnum}");
                    break;

                case '-':
                    Console.WriteLine($"{firstnum} - {secondnum} = {firstnum - secondnum}");
                    break;

                case '*':
                    Console.WriteLine($"{firstnum} * {secondnum} = {firstnum * secondnum}");
                    break;

                case '/':
                    if (secondnum != 0)
                    {
                        Console.WriteLine($"{firstnum} / {secondnum} = {firstnum / secondnum}");
                    }
                    else
                    {
                        Console.WriteLine("Error: Division by zero is not allowed.");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid operation entered.");
                    break;
            }

            Console.Read();
        }
    }
}

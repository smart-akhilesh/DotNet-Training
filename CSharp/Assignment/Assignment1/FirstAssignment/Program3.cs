using System;

namespace FirstAssignment
{
    class Program3
    {
        public static void Main()
        {
            Console.WriteLine("Program to perform different operation");
            Console.Write("Enter first number : ");
            int firstnum = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter operation (+, -, *, /) : ");
            char operation = Convert.ToChar(Console.ReadLine());
            Console.Write("Enter second number : ");
            int secondnum = Convert.ToInt32(Console.ReadLine());

            switch (operation)
            {
                case '+':
                    Console.WriteLine($"Sum : {firstnum} + {secondnum} = {firstnum + secondnum}");
                    break;

                case '-':
                    Console.WriteLine($"Difference : {firstnum} - {secondnum} = {firstnum - secondnum}");
                    break;

                case '*':
                    Console.WriteLine($"Multiplication : {firstnum} * {secondnum} = {firstnum * secondnum}");
                    break;

                case '/':
                    if (secondnum != 0)
                    {
                        Console.WriteLine($"Division : {firstnum} / {secondnum} = {firstnum / secondnum}");
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

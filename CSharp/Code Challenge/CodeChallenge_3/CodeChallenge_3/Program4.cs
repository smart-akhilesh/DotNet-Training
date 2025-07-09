using System;

namespace CodeChallenge_3
{
    public delegate int CalculatorDelegate(int a, int b);

    class Calculator
    {
        public static int Add(int a, int b) => a + b;
        public static int Subtract(int a, int b) => a - b;
        public static int Multiply(int a, int b) => a * b;

        public static int Divide(int a, int b)
        {
           
            return a / b;
        }
    }

    class Program4
    {
        static void Operation(int a, int b, CalculatorDelegate operation, string operationName)
        {
            try
            {
                int result = operation(a, b);
                Console.WriteLine($"{operationName} is: {result}");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Error: Division by zero is not allowed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during {operationName.ToLower()}: {ex.Message}");
            }
        }

        public static void Main()
        {
            try
            {
                Console.Write("Enter first integer: ");
                int num1 = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter second integer: ");
                int num2 = Convert.ToInt32(Console.ReadLine());

                Operation(num1, num2, Calculator.Add, "Addition");
                Operation(num1, num2, Calculator.Subtract, "Subtraction");
                Operation(num1, num2, Calculator.Multiply, "Multiplication");
                Operation(num1, num2, Calculator.Divide, "Division");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter valid integers.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong");
            }

            Console.Read();
        }
    }
}

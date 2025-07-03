using System;

namespace CodeChallenge2
{
    class NegativeValueException : Exception
    {
        public NegativeValueException(string message) : base(message) { }
    }

    class Program
    {
        static void CheckPositive(int number)
        {
            if (number < 0)
                throw new NegativeValueException("Number is negative. Please enter a positive number.");
        }

        static void Main(string[] args)
        {
            try
            {
                Console.Write("Enter an integer: ");
                int number = int.Parse(Console.ReadLine());

                CheckPositive(number);

            }
            catch (NegativeValueException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
            }
            finally
            {
                Console.WriteLine("Program execution finished.");
            }

            Console.Read();
        }
    }
}

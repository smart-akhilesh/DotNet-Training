using System;

namespace Assignment5
{
    public class Scholarship
    {
        public double Merit(double marks, double fees)
        {
            if (marks >= 70 && marks <= 80)
                return fees * 0.20;
            else if (marks > 80 && marks <= 90)
                return fees * 0.30;
            else if (marks > 90)
                return fees * 0.50;
            else
                throw new InvalidMarkException("Marks is not qualify for a scholarship. Better Luck next time.");
        }
    }

    public class InvalidMarkException : Exception
    {
        public InvalidMarkException(string message) : base(message)
        {
        }
    }

    public class Program2
    {
        public static void Main()
        {
            try
            {
                Scholarship scholarship = new Scholarship();

                Console.Write("Enter marks: ");
                double marks = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter fees: ");
                double fees = Convert.ToDouble(Console.ReadLine());

                double scholarshipAmount = scholarship.Merit(marks, fees);
                Console.WriteLine($"Scholarship Amount: {scholarshipAmount}");


            }
            catch (InvalidMarkException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter valid numbers for marks and fees.");
            }

            Console.Read();
        }
    }
}
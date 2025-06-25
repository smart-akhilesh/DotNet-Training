/*
Scenario:
You are developing a C# application to calculate students' final grades. The application takes
user input for marks in three subjects. The input should be integers between 0 and 100.
You need to ensure that:
The input is a valid integer.
The marks are within the valid range.
The average is calculated and displayed.
Question:
Design a try-catch block that:
Handles non-integer inputs using FormatException.
Handles invalid range(e.g., negative numbers or >100) using ArgumentOutOfRangeException.
Always displays a "Grade calculation complete" message using finally.

Explain how exception handling improves robustness in this scenario and optionally 
suggest how custom exceptions could be used to further improve it.

*/

using System;

namespace CodeChallenge1
{
    class Program2
    {
        public static void Main()
        {
            int[] marks = new int[3];
            int total = 0;

            try
            {
                for (int i = 0; i < marks.Length; i++)
                {
                    Console.Write($"Enter marks for subject {i + 1}: ");
                    marks[i] = Convert.ToInt32(Console.ReadLine());

                    if (marks[i] < 0 || marks[i] > 100)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    total += marks[i];
                }

                Console.WriteLine($"Average marks: {total / 3.0}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter an integer value.");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Marks must be between 0 and 100.");
            }
            finally
            {
                Console.WriteLine("Grade calculation complete");
            }
            Console.Read();
        }
    }
}




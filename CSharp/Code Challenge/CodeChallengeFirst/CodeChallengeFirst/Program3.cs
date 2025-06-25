/*
3.Write a C# Sharp program to check the largest number among three given integers.
 
Sample Input:
1,2,3
1,3,2
1,1,1
1,2,2
Expected Output:
3
3
1
2
  */

using System;

namespace CodeChallengeFirst
{
    class Program3
    {
        public static void Main()
        {
            Console.Write("Enter first number: ");
            int num1 = Convert.ToInt32(Console.ReadLine();
            Console.Write("Enter second number: ");
            int num2 = Convert.ToInt32(Console.ReadLine();
            Console.Write("Enter third number: ");
            int num3 = Convert.ToInt32(Console.ReadLine());
            int largest;
            if (num1 >= num2 && num1 >= num3)
            {
                largest = num1;
            }
            else if (num2 >= num1 && num2 >= num3)
            {
                largest = num2;
            }
            else
            {
                largest = num3;
            }

            Console.WriteLine("Largest number is: " + largest);
            Console.Read();
        }
    }
}




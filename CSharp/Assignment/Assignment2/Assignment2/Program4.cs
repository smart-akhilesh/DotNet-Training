using System;


namespace Assignment2
{
    class Program4
    {
        public static void Main()
        {
            Console.WriteLine("Program to find average, minimum and maximum");
            int[] arr = { 2, 3, 4, 4, 6, 5, 5 };
            TotalAverMinMAx(arr);
            Console.Read();
        }

        static void TotalAverMinMAx(int[] arr)
        {
            int min = arr[0];
            int max = arr[0];
            int sum = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > max)
                {
                    max = arr[i];
                }
                if (arr[i] < min)
                {
                    min = arr[i];
                }
                sum = sum + arr[i];
            }
            Console.WriteLine("Average : {0}", sum / arr.Length);
            Console.WriteLine("Maximum mark : {0}", max);
            Console.WriteLine("Minimum mark : {0}", min);
        }

    }
}

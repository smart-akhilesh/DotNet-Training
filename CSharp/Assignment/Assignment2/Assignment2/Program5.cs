using System;


namespace Assignment2
{
    class Program5
    {
        public static void Main()
        {
            Console.WriteLine("Program to print total, average, minimum, maximum, ascending and descending");
            int[] arr = new int[10];
            InputArray(arr);
            TotalAverMinMAx(arr);
            Array.Sort(arr);
            Ascending(arr);
            Descending(arr);
            Console.Read();
        }

        static void InputArray(int[] arr)
        {
            Console.Write("Input Ten marks : ");
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }
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
            
            Console.WriteLine("Total : {0}", sum);
            Console.WriteLine("Average : {0}", sum / arr.Length);
            Console.WriteLine("Maximum mark : {0}", max);
            Console.WriteLine("Minimum mark : {0}",min);
        }
        static void Ascending(int[] arr)
        {
            Console.Write("Marks in Ascending order : ");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write("{0} ",arr[i]);
            }
            Console.WriteLine();
        }

        static void Descending(int[] arr)
        {
            Console.Write("Marks in Descending order : ");
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                Console.Write("{0} ", arr[i]);
            }
        }
    }
}

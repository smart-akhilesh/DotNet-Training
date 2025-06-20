using System;


namespace Assignment2
{
    class Program6
    {
        public static void Main()
        {
            Console.WriteLine("Program to copy the data of one array into another array");
            Console.WriteLine();
            int[] arr1 = { 2, 3, 4, 66, 3, 6, 3 };
            int[] arr2 = new int[arr1.Length];
            CopyFirstArrToSec(arr1, arr2);
            DisplayArraySec(arr2);
            Console.Read();
        }

        static void CopyFirstArrToSec(int[] arr, int[] arr2)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr2[i] = arr[i];
            }
        }

        static void DisplayArraySec(int[] arr2)
        {
            Console.Write("Second array data after copy : ");
            for (int i = 0; i < arr2.Length; i++)
            {
                Console.Write("{0} ",arr2[i]);
            }
        }
    }
}

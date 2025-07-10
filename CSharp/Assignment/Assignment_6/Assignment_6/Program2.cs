using System;
using System.IO;

namespace Assignment_6
{
    class Program2
    {
        static void Main()
        {
            string filePath = @"C:\Users\akhileshsing\OneDrive - Infinite Computer Solutions (India) Limited\Documents\ArrayFile.txt";
            int lineCount = 0;

            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        while (reader.ReadLine() != null)
                        {
                            lineCount++;
                        }
                    }
                    Console.WriteLine($"Number of lines in the file: {lineCount}");
                }
                else
                {
                    Console.WriteLine($"File '{filePath}' does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading the file: {ex.Message}");
            }
            Console.Read();
        }
    }
}

using System;
using System.IO;

namespace Assignment_6
{
    class Program
    {
        static void Main()
        {
            string[] lines = { "Hello", "Welcome to C# programming", "This is file write example" };
            string filePath = @"C:\Users\akhileshsing\OneDrive - Infinite Computer Solutions (India) Limited\Documents\ArrayFile.txt";

            try
            {
                
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        foreach (string line in lines)
                        {
                            writer.WriteLine(line);
                        }
                    }
                }

                Console.WriteLine($"Array of strings successfully written to file '{filePath}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

}

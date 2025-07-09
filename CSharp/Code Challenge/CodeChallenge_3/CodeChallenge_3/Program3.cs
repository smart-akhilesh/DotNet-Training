using System;
using System.IO;

namespace CodeChallenge_3
{
    class Program3
    {
        public static void AppendTextToFile()
        {
            string filePath = @"C:\Users\akhileshsing\OneDrive - Infinite Computer Solutions (India) Limited\Documents\AppendedFile.txt";
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                sw = new StreamWriter(fs);

                Console.WriteLine("Enter text to append to the file:");
                string userInput = Console.ReadLine();

                sw.WriteLine(userInput);
                Console.WriteLine("Text appended successfully.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory does not exist.");
            }
         
            catch (IOException ex)
            {
                Console.WriteLine("IO Error: " + ex.Message);
            }
            catch (Exception)
            {
                Console.WriteLine("Something wrong");
            }
            finally
            {
               
                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }
        }

        static void Main(string[] args)
        {
            AppendTextToFile();
            Console.Read();
        }
    }
}

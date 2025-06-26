using System;

namespace Assignment3
{
    class Student
    {
        int RollNo;
        string Name;
        int Class;
        int Semester;
        string Branch;
        int[] marks = new int[5];
        string result;

        public Student(int RollNo, string Name, int Class, int Semester, string Branch)
        {
            this.RollNo = RollNo;
            this.Name = Name;
            this.Class = Class;
            this.Semester = Semester;
            this.Branch = Branch;
        }

        public void GetMarks()
        {
            Console.WriteLine("Enter marks for 5 subjects:");
            for (int i = 0; i < marks.Length; i++)
            {
                Console.Write($"Subject {i + 1}: ");
                marks[i] = Convert.ToInt32(Console.ReadLine());
            }
        }

        public void DisplayResult()
        {
            int sum = 0;
            bool anySubjectFailed = false;

            for (int i = 0; i < marks.Length; i++)
            {
                if (marks[i] < 35)
                {
                    anySubjectFailed = true;
                }
                sum += marks[i];
            }

            float average = sum / 5.0f;

            if (anySubjectFailed)
            {
                result = "Failed";
            }
            else if (average < 50)
            {
                result = "Failed";
            }
            else
            {
                result = "Passed";
            }

            Console.WriteLine("Result Status: " + result);
        }

        public void DisplayData()
        {
            Console.WriteLine("\n--- Student Details ---");
            Console.WriteLine($"Roll No: {RollNo}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Class: {Class}");
            Console.WriteLine($"Semester: {Semester}");
            Console.WriteLine($"Branch: {Branch}");
            Console.WriteLine("Marks:");
            for (int i = 0; i < marks.Length; i++)
            {
                Console.WriteLine($"Subject {i + 1}: {marks[i]}");
            }
            Console.WriteLine("Final Result: " + result);
        }
    }

    class Program2
    {
        static void Main(string[] args)
        {
            Student s1 = new Student(101, "Alice", 10, 2, "Computer Science");
            s1.GetMarks();
            s1.DisplayResult();
            s1.DisplayData();

            Console.ReadLine();
        }
    }
}


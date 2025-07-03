/*
1. Create an Abstract class Student with  Name, StudentId, Grade as members and also an abstract 
method Boolean Ispassed(grade) which takes grade as an input and checks whether student passed the course or not.  
 
Create 2 Sub classes Undergraduate and Graduate that inherits all members of the student and overrides 
Ispassed(grade) method
 
For the UnderGrad class, if the grade is above 70.0, then isPassed returns true, otherwise it returns 
false. For the Grad class, if the grade is above 80.0, then isPassed returns true, otherwise returns false.
 
Test the above by creating appropriate objects
*/

using System;

namespace StudentGrading
{
    abstract class Student
    {
        public string Name { get; set; }
        public int StudentId { get; set; }
        public double Grade { get; set; }

        public abstract bool IsPassed(double grade);
    }

    class Undergraduate : Student
    {
        public override bool IsPassed(double grade) => grade > 70.0;
    }

    class Graduate : Student
    {
        public override bool IsPassed(double grade) => grade > 80.0;
    }

    class Program
    {
        static void Main(string[] args)
        {
            ProcessStudent(new Undergraduate(), "Undergraduate");
            Console.WriteLine();
            ProcessStudent(new Graduate(), "Graduate");

            Console.Read();
        }

        static void ProcessStudent(Student student, string type)
        {
            Console.Write($"Enter {type} Student Name: ");
            student.Name = Console.ReadLine();

            Console.Write($"Enter {type} Student ID: ");
            student.StudentId = Convert.ToInt32(Console.ReadLine());

            Console.Write($"Enter {type} Student Grade: ");
            student.Grade = Convert.ToDouble(Console.ReadLine());

            string result = student.IsPassed(student.Grade) ? "Passed" : "Failed";

            Console.WriteLine($"{type} Student: {student.Name}, Grade: {student.Grade}, Result: {result}");
        }
    }
}


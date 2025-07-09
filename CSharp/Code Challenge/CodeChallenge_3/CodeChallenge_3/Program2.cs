using System;

namespace CodeChallenge_3
{
    class Box
    {
        public int Length { get; set; }
        public int Breadth { get; set; }

        public Box(int length, int breadth)
        {
            Length = length;
            Breadth = breadth;
        }

        public static Box operator +(Box b1, Box b2)
        {
            return new Box(b1.Length + b2.Length, b1.Breadth + b2.Breadth);
        }

        public void Display()
        {
            Console.WriteLine($"Length: {Length}, Breadth: {Breadth}");
        }
    }

    class Program2
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter the Length of Box 1:");
                int length1 = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter the Breadth of Box 1:");
                int breadth1 = Convert.ToInt32(Console.ReadLine());
                Box box1 = new Box(length1, breadth1);

                Console.WriteLine("Enter the Length of Box 2:");
                int length2 = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter the Breadth of Box 2:");
                int breadth2 = Convert.ToInt32(Console.ReadLine());
                Box box2 = new Box(length2, breadth2);

                Box box3 = box1 + box2;

                Console.WriteLine("Details of the resulting box after addition:");
                box3.Display();
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter integer values only.");
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
            }

            Console.Read();
        }
    }
}

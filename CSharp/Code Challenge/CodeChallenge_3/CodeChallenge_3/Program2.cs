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
            int length, breadth;
            Console.WriteLine("Enter the Lenght of Box 1");
            length = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the Breadth of Box 1");
            breadth = Convert.ToInt32(Console.ReadLine());
            Box box1 = new Box(length, breadth);

            Console.WriteLine("Enter the Lenght of Box 2");
            length = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the Breadth of Box 1");
            breadth = Convert.ToInt32(Console.ReadLine());
           
            Box box2 = new Box(length, breadth);

    
            Box box3 = box1 + box2;

            Console.WriteLine("Details of the resulting box after addition:");
            box3.Display();

            Console.Read();
        }
    }
}

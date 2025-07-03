/*2. Create a Class called Products with Productid, Product Name, Price. Accept 10 Products, 
 * sort them based on the price, and display the sorted Products
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChallenge2
{
    class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }

        public void Display() =>
            Console.WriteLine($"ID: {ProductId}, Name: {ProductName}, Price: {Price}");
    }

    class Program3
    {
        public static void Main()
        {
            var products = new List<Product>();

            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"\nProduct {i}:");

                Console.Write("ID: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Price: ");
                int price = int.Parse(Console.ReadLine());

                products.Add(new Product { ProductId = id, ProductName = name, Price = price });
            }

            var sorted = products.OrderBy(p => p.Price);

            Console.WriteLine("\nProducts sorted by Price:");
            foreach (var p in sorted)
                p.Display();
        }
    }
}

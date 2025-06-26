using System;

namespace Assignment3
{
    class SalesDetails
    {
        int SalesNo;
        int ProductNo;
        int Price;
        DateTime DateOFSale;
        int Quantity;
        int TotalAmount;

        public void CalculateSales(int Quantity, int Price)
        {
            TotalAmount = Quantity * Price;

        }

        public SalesDetails(int SalesNo, int ProductNo, int Price, int Quantity, DateTime DateOfSale)
        {
            this.SalesNo = SalesNo;
            this.ProductNo = ProductNo;
            this.Price = Price;
            this.Quantity = Quantity;
            this.DateOFSale = DateOfSale;
            CalculateSales(Quantity, Price);

        }

        public void ShowDetails()
        {
            Console.WriteLine($"Sales No. : {SalesNo}  Production No : {ProductNo}  Price : {Price}  DateOFSale  : {DateOFSale}  Quantity : {Quantity}  Total Amount : {TotalAmount}");
        }
    }
    class Program3
    {
        public static void Main()
        {
            SalesDetails sd = new SalesDetails(123, 23, 1000, 2, Convert.ToDateTime("06/25/2025"));
            sd.ShowDetails();
            Console.Read();
        }
    }
}


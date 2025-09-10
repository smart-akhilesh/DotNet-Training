using System;

namespace Ecommerce_Project.DTO
{
    public class ProductDTO
    {
        public int ProductID { get; set; }
        public int RetailerID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int RemainingQuantity { get; set; }
        public int QuantityAvailable { get; set; }

        // URL of the product image entered by retailer
        public string ImageURL { get; set; }
    }
}

using System;

namespace Ecommerce_Project.DTO
{
    public class ProductViewDTO
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public int RemainingQuantity { get; set; }   // ✅ new column mapped
        public DateTime CreatedAt { get; set; }
        public string ImageURL { get; set; }         // ✅ match Products table
    }
}

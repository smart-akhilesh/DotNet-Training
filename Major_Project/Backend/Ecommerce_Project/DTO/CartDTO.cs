namespace Ecommerce_Project.DTO
{
    public class CartDTO
    {
        public int CartID { get; set; }           // Unique identifier for the cart item
        public int UserID { get; set; }           // ID of the user who owns the cart
        public int ProductID { get; set; }        // ID of the product added to the cart
        public string ProductName { get; set; }   // Name of the product
        public decimal Price { get; set; }        // Price of the product
        public int Quantity { get; set; }         // Quantity selected by the user
        public string ImageURL { get; set; }      // URL of the product image
        public int RemainingQuantity { get; set; } // ✅ Remaining quantity available for the product
    }
}

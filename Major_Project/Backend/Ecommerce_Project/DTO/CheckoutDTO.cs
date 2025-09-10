using System.Collections.Generic;

namespace Ecommerce_Project.DTO
{
    public class CheckoutDTO
    {
        public int UserID { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingMethod { get; set; }
        public List<CartItemDTO> CartItems { get; set; }
    }

    public class CartItemDTO
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}

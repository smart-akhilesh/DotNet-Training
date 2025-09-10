using System;
using System.Collections.Generic;

namespace ECommerce.DTO
{
    public class CheckoutDTO
    {
        public int UserID { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public List<CartDTO> CartItems { get; set; } = new List<CartDTO>();
    }
}

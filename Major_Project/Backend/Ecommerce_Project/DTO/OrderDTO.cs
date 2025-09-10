using System;
using System.Collections.Generic;

namespace Ecommerce_Project.DTO
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();

        // ✅ Add this so controller code compiles
        public ShippingDTO Shipping { get; set; }
    }

    public class OrderItemDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class ShippingDTO
    {
        public int ShippingID { get; set; }
        public string Status { get; set; }
        public string TrackingNumber { get; set; }
        public string ShippingMethod { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

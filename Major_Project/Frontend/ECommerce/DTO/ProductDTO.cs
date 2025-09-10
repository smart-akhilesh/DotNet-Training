using System;

namespace ECommerce.DTO
{
    public class ProductDTO
    {
        public int ProductID { get; set; }       
        public int RetailerID { get; set; }      
        public int CategoryID { get; set; }      

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }

        public int RemainingQuantity { get; set; }  

  
        public string ImageURL { get; set; }
    }
}

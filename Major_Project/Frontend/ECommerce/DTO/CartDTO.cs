using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.DTO
{
    public class CartDTO
    {
        public int CartID { get; set; }           
        public int UserID { get; set; }          
        public int ProductID { get; set; }        
        public string ProductName { get; set; }   
        public decimal Price { get; set; }     
        public int Quantity { get; set; }        
        public string ImageURL { get; set; }      
    }
}
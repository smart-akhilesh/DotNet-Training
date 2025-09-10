using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_Project.DTO
{
    public class CategoriesDTO
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
    }
}
using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;

namespace ECommerce.DTO

{


    public class WhishlistDTO
    {
        public int WishlistID { get; set; }
        public int? UserID { get; set; }
        public int? ProductID { get; set; }
        public DateTime? AddedAt { get; set; }

     
        public string ProductName { get; set; }
        public string ProductImageURL { get; set; }
        public decimal ProductPrice { get; set; }
    }


}

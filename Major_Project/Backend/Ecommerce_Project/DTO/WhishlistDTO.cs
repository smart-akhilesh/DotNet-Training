using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;

namespace Ecommerce_Project.DTO

{

    public class WhishlistDTO

    {

     
        public Nullable<int> UserID { get; set; }

        public Nullable<int> ProductID { get; set; }

        public Nullable<System.DateTime> AddedAt { get; set; }

    }

}

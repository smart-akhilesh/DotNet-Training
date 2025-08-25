using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeChallenge_9.Controllers
{
    public class CodeController : Controller
    {
       
        northwindEntities db = new northwindEntities();

    
        public ActionResult GermanCustomers()
        {
            var customers = db.Customers
                              .Where(c => c.Country == "Germany")
                              .ToList();

            return View(customers);
        }

      
        public ActionResult CustomerByOrder()
        {
            var customer = (from o in db.Orders
                            where o.OrderID == 10248
                            select o.Customer).FirstOrDefault();

            return View(customer);
        }
    }
}
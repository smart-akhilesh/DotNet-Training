using MVC_Web.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace MVC_Web.Controllers
{
    public class OrdersController : ApiController
    {
        private northwindEntities northwind = new northwindEntities();

        [HttpGet]
        [Route("api/orders/employee/{id}")]
        public IHttpActionResult GetOrdersByEmployee(int id)
        {
            var orders = northwind.Orders
                           .Where(o => o.EmployeeID == id)
                           .Select(o => new
                           {
                               o.OrderID,
                               o.OrderDate,
                               o.RequiredDate,
                               o.ShippedDate,
                               o.ShipName,
                               o.ShipCity,
                               o.ShipCountry
                           })
                           .ToList();

            if (orders == null)
                return NotFound();

            return Ok(orders);
        }

        [HttpGet]
        [Route("api/customers/bycountry/{country}")]
        public IHttpActionResult GetCustomersByCountry(string country)
        {
            var customers = northwind.GetCustomersByCountry(country); 

            if (customers == null)
                return NotFound();

            return Ok(customers);
        }

    }
}

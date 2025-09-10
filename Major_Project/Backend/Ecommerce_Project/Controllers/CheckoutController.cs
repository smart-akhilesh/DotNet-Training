using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ecommerce_Project.DTO;
using Ecommerce_Project.Models;

namespace Ecommerce_Project.Controllers
{
    public class CheckoutController : ApiController
    {

        majorprojectEntities db = new majorprojectEntities();
        [HttpPost]
        [Route("api/checkout")]
        public IHttpActionResult Checkout([FromBody] CheckoutDTO checkoutData)
        {
            try
            {
                if (checkoutData == null || checkoutData.CartItems == null || !checkoutData.CartItems.Any())
                    return BadRequest("Invalid checkout data.");

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1️⃣ Create Order
                        var order = new Order
                        {
                            UserID = checkoutData.UserID,
                            TotalAmount = checkoutData.CartItems.Sum(c => c.Price * c.Quantity),
                            OrderDate = DateTime.Now
                        };
                        db.Orders.Add(order);
                        db.SaveChanges();

                        // 2️⃣ Create OrderDetails
                        foreach (var item in checkoutData.CartItems)
                        {
                            var orderDetail = new OrderDetail
                            {
                                OrderID = order.OrderID,
                                ProductID = item.ProductID,
                                Quantity = item.Quantity,
                                PriceAtPurchase = item.Price
                            };
                            db.OrderDetails.Add(orderDetail);
                        }
                        db.SaveChanges();

                        // 3️⃣ Save Addresses (NEW)
                        var address = new OrderAddress
                        {
                            OrderID = order.OrderID,
                            BillingAddress = checkoutData.BillingAddress,
                            ShippingAddress = checkoutData.ShippingAddress
                        };
                        db.OrderAddresses.Add(address);
                        db.SaveChanges();

                        // 4️⃣ Create ShippingDetails
                        var shipping = new ShippingDetail
                        {
                            OrderID = order.OrderID,
                            Status = "Pending",
                            TrackingNumber = null,
                            ShippingMethod = checkoutData.ShippingMethod,
                            EstimatedDeliveryDate = DateTime.Now.AddDays(7),
                            LastUpdated = DateTime.Now
                        };
                        db.ShippingDetails.Add(shipping);
                        db.SaveChanges();

                        // 5️⃣ Remove Cart Items for this User
                        var userCartItems = db.Carts.Where(c => c.UserID == checkoutData.UserID).ToList();
                        if (userCartItems.Any())
                        {
                            db.Carts.RemoveRange(userCartItems);
                            db.SaveChanges();
                        }

                        transaction.Commit();

                        return Ok(new { Message = "Order placed successfully!", OrderID = order.OrderID });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return InternalServerError(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}

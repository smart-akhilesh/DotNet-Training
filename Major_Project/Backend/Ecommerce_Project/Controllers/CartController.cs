using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ecommerce_Project.Models;
using Ecommerce_Project.DTO;

namespace Ecommerce_Project.Controllers
{
    public class CartController : ApiController
    {
        majorprojectEntities db = new majorprojectEntities();

        // GET: api/cart/{userId}
        [HttpGet]
        [Route("api/cart/{userId}")]
        public IHttpActionResult GetCartItems(int userId)
        {
            try
            {
                var cartItems = db.Carts
                    .Where(c => c.UserID == userId)
                    .Select(c => new CartDTO
                    {
                        CartID = c.CartID,
                        ProductID = (int)c.ProductID,
                        ProductName = c.Product.Name,
                        Price = (decimal)c.Product.Price,
                        Quantity = (int)c.Quantity,
                        ImageURL = c.Product.ImageURL  // Directly from Products table
            })
                    .ToList();

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/cart")]
        public IHttpActionResult AddToCart(CartDTO cartDto)
        {
            try
            {
                var product = db.Products.Find(cartDto.ProductID);
                if (product == null)
                    return NotFound();

                // Check RemainingQuantity
                int currentCartQuantity = db.Carts
                    .Where(c => c.UserID == cartDto.UserID && c.ProductID == cartDto.ProductID)
                    .Select(c =>  c.Quantity)
                    .FirstOrDefault() ?? 0;

                if (cartDto.Quantity + currentCartQuantity > product.RemainingQuantity)
                {
                    return BadRequest($"Cannot add more than {product.RemainingQuantity} items. Current in cart: {currentCartQuantity}");
                }

                var existingItem = db.Carts.FirstOrDefault(c =>
                    c.UserID == cartDto.UserID && c.ProductID == cartDto.ProductID);

                if (existingItem != null)
                {
                    existingItem.Quantity += cartDto.Quantity;
                }
                else
                {
                    Cart newItem = new Cart
                    {
                        UserID = cartDto.UserID,
                        ProductID = cartDto.ProductID,
                        Quantity = cartDto.Quantity
                    };
                    db.Carts.Add(newItem);
                }

                db.SaveChanges();
                return Ok(new { message = "Item added to cart." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/cart/{cartId}")]
        public IHttpActionResult UpdateCartItem(int cartId, [FromBody] int quantity)
        {
            try
            {
                var cartItem = db.Carts.Find(cartId);
                if (cartItem == null)
                    return NotFound();

                var product = db.Products.Find(cartItem.ProductID);
                if (product == null)
                    return BadRequest("Product not found.");

                int remaining = (product.RemainingQuantity ?? 0);

                if (quantity > remaining)
                    return BadRequest("Cannot add more than remaining quantity.");

                cartItem.Quantity = quantity;
                db.SaveChanges();

                return Ok(new { message = "Cart item updated." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpDelete]
        [Route("api/cart/{cartId}")]
        public IHttpActionResult RemoveCartItem(int cartId)
        {
            try
            {
                var cartItem = db.Carts.Find(cartId);
                if (cartItem == null)
                    return NotFound();

                db.Carts.Remove(cartItem);
                db.SaveChanges();

                return Ok(new { message = "Item removed from cart." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Route("api/orders/{userId}")]
        public IHttpActionResult GetOrdersByUser(int userId)
        {
            try
            {
                var orders = db.Orders
                    .Where(o => o.UserID == userId)
                    .Select(o => new OrderDTO
                    {
                        OrderID = o.OrderID,
                        UserID = (int)o.UserID,
                        OrderDate = (DateTime)o.OrderDate,
                        TotalAmount = (decimal)o.TotalAmount,

                // ✅ Map OrderDetails
                Items = db.OrderDetails
                                  .Where(od => od.OrderID == o.OrderID)
                                  .Select(od => new OrderItemDTO
                                  {
                                      ProductID = (int)od.ProductID,
                                      ProductName = od.Product.Name,
                                      ImageURL = od.Product.ImageURL,
                                      Price = (decimal)od.PriceAtPurchase,
                                      Quantity = (int)od.Quantity
                                  })
                                  .ToList(),

                // ✅ Map ShippingDetails
                Shipping = db.ShippingDetails
                                     .Where(s => s.OrderID == o.OrderID)
                                     .Select(s => new ShippingDTO
                                     {
                                         ShippingID = s.ShippingID,
                                         Status = s.Status,
                                         TrackingNumber = s.TrackingNumber,
                                         ShippingMethod = s.ShippingMethod,
                                         EstimatedDeliveryDate = (DateTime)s.EstimatedDeliveryDate,
                                         LastUpdated = (DateTime)s.LastUpdated
                                     })
                                     .FirstOrDefault() // one shipping per order
            })
                    .ToList();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


    }
}

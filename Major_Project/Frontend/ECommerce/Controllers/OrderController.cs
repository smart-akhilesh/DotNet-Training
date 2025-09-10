using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ECommerce.DTO;
using Newtonsoft.Json;

namespace ECommerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly string apiBaseUrl = "https://localhost:44353/";

        [HttpGet]
        public ActionResult Checkout()
        {
            
            var checkoutData = new CheckoutDTO();

            if (Session["Cart"] != null)
            {
                checkoutData.CartItems = (List<CartDTO>)Session["Cart"];
            }

            return View(checkoutData);
        }

        [HttpPost]
        public async Task<ActionResult> PlaceOrder(CheckoutDTO checkoutData)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Signin", "Account");

            checkoutData.UserID = Convert.ToInt32(Session["UserID"]);

         
            List<CartDTO> cartItems = new List<CartDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44353/");
                var response = await client.GetAsync($"api/cart/{checkoutData.UserID}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    cartItems = JsonConvert.DeserializeObject<List<CartDTO>>(json);
                }
            }

            // attach cart items
            checkoutData.CartItems = cartItems;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44353/");
                var content = new StringContent(JsonConvert.SerializeObject(checkoutData), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/checkout", content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(json);
                    int orderId = result.OrderID;

                    return RedirectToAction("OrderSuccess", new { id = orderId });
                }
                else
                {
                    ViewBag.Message = "Error placing order.";
                    return View("Checkout", checkoutData);
                }
            }
        }

        public ActionResult OrderSuccess(int id)
        {
            ViewBag.OrderID = id;
            return View();
        }

    }
}

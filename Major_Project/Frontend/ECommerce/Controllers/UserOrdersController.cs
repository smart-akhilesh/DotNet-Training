using ECommerce.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class UserOrdersController : Controller
    {
        private readonly string apiBaseUrl = "https://localhost:44353/";

        private int GetUserId()
        {
            
            if (Session["UserID"] == null)
            {
                Response.Redirect("https://localhost:44324/Account/Signin?role=User");
                return 0;
            }

            return Convert.ToInt32(Session["UserID"]);
        }

        public async Task<ActionResult> Index()
        {
            int userId = GetUserId();
            if (userId == 0) return null; 

            var orders = new List<OrderDTO>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );

                var response = await client.GetAsync($"api/orders/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    orders = JsonConvert.DeserializeObject<List<OrderDTO>>(jsonString)
                             ?? new List<OrderDTO>();
                }
                else
                {
                    ModelState.AddModelError("", $"API error: {response.StatusCode}");
                }
            }

            return View(orders);
        }

      
        public async Task<ActionResult> MyOrders()
        {
            return await Index();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using ECommerce.DTO;
using Newtonsoft.Json;

namespace ECommerce.Controllers
{
    public class AddtoCartController : Controller
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
            List<CartDTO> cartItems = new List<CartDTO>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/cart/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    cartItems = JsonConvert.DeserializeObject<List<CartDTO>>(json);
                }
            }

            return View(cartItems);
        }

      
        public async Task<ActionResult> Add(int id)
        {
            var product = await GetProductByIdAsync(id);
            if (product != null)
            {
                var cartDto = new CartDTO
                {
                    UserID = GetUserId(),
                    ProductID = product.ProductID,
                    Quantity = 1,
                    Price = product.Price,
                    ProductName = product.Name,
                    ImageURL = product.ImageURL
                };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(JsonConvert.SerializeObject(cartDto), System.Text.Encoding.UTF8, "application/json");
                    await client.PostAsync("api/cart", content);
                }
            }

            return RedirectToAction("Index");
        }

      
        public async Task<ActionResult> Remove(int cartId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                await client.DeleteAsync($"api/cart/{cartId}");
            }

            return RedirectToAction("Index");
        }

     
        public async Task<ActionResult> UpdateQuantity(int cartId, int quantity)
        {
            if (quantity < 1)
                quantity = 1;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                var content = new StringContent(JsonConvert.SerializeObject(quantity), System.Text.Encoding.UTF8, "application/json");
                await client.PutAsync($"api/cart/{cartId}", content);
            }

            return RedirectToAction("Index");
        }

      
        private async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/products/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ProductDTO>(json);
                }
            }

            return null;
        }
    }
}

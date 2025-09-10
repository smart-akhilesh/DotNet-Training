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
    public class WishlistController : Controller
    {
        private readonly string apiBaseUrl = "https://localhost:44353/";

        private int GetUserId()
        {
            // ✅ Ensure user is logged in, otherwise redirect to Signin
            if (Session["UserID"] == null)
            {
                Response.Redirect("https://localhost:44324/Account/Signin?role=User");
                return 0;
            }

            return Convert.ToInt32(Session["UserID"]);
        }

        // GET: Wishlist
        public async Task<ActionResult> Index()
        {
            int userId = GetUserId();
            List<WhishlistDTO> items = new List<WhishlistDTO>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/whishlist/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    items = JsonConvert.DeserializeObject<List<WhishlistDTO>>(json);
                }
            }

            // Fetch product details for each wishlist item
            foreach (var item in items)
            {
                if (item.ProductID.HasValue)
                {
                    var product = await GetProductByIdAsync(item.ProductID.Value);
                    if (product != null)
                    {
                        item.ProductName = product.Name;
                        item.ProductImageURL = product.ImageURL;
                        item.ProductPrice = product.Price;
                    }
                }
            }

            return View(items);
        }

        // Add product to wishlist
        public async Task<ActionResult> Add(int id)
        {
            var product = await GetProductByIdAsync(id);
            if (product != null)
            {
                var wishlistItem = new WhishlistDTO
                {
                    UserID = GetUserId(),
                    ProductID = product.ProductID
                };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(JsonConvert.SerializeObject(wishlistItem), System.Text.Encoding.UTF8, "application/json");
                    await client.PostAsync("api/whishlist", content);
                }
            }

            return RedirectToAction("Index");
        }

        // Remove product from wishlist
        public async Task<ActionResult> Remove(int Id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                await client.DeleteAsync($"api/whishlist/{Id}");
            }

            return RedirectToAction("Index");
        }

        // Fetch product details from API
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

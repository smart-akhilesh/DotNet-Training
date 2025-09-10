using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using ECommerce.DTO;
using Newtonsoft.Json;

namespace ECommerce.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly string apiBaseUrl = "https://localhost:44353/";

        public async Task<ActionResult> Index(int id)
        {
            ProductDTO product = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync($"api/products/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        product = JsonConvert.DeserializeObject<ProductDTO>(json);

                        if (product != null && product.RemainingQuantity == 0)
                        {
                            product.RemainingQuantity = product.QuantityAvailable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error fetching product: {ex.Message}");
            }

            if (product == null)
            {
                return HttpNotFound("Product not found");
            }

            return View(product);
        }
    }
}

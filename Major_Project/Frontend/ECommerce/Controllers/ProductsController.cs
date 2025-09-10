using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using ECommerce.DTO;
using System.Collections.Generic;
using System;
using ECommerce.Models;
using System.Linq;
namespace ECommerce.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        private readonly majorprojectEntities db = new majorprojectEntities();
        public async Task<ActionResult> Index()
        {
            List<ProductDTO> products = new List<ProductDTO>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44353/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/products");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<List<ProductDTO>>(jsonString);
                }
            }

            return View(products);
        }

        public ActionResult Category(string category, int page = 1, string sort = "name")

        {

            int pageSize = 9;

            var products = db.Products

                .Where(p => p.Category.CategoryName == category);

            // Sorting

            switch (sort)

            {

                case "price":

                    products = products.OrderBy(p => p.Price);

                    break;

                case "newest":

                    products = products.OrderByDescending(p => p.CreatedAt);

                    break;

                default:

                    products = products.OrderBy(p => p.Name);

                    break;

            }

            var pagedProducts = products

                .Skip((page - 1) * pageSize)

                .Take(pageSize)

                .ToList();

            ViewBag.CategoryName = category;

            ViewBag.CurrentPage = page;

            ViewBag.TotalPages = (int)Math.Ceiling((double)products.Count() / pageSize);

            ViewBag.Sort = sort;

            return View("Category", pagedProducts);

        }
        public ActionResult Search(string query)

        {

            var results = db.Products.Where(p => p.Name.Contains(query) || p.Description.Contains(query))

            .ToList();

            ViewBag.SearchQuery = query;

            return View("SearchResults", results);

        }

      


    }
}
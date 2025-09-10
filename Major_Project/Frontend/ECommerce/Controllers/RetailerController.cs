using ECommerce.DTO;
using ECommerce.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    [Authorize]
    public class RetailerController : Controller
    {
        private readonly string apiBaseUrl = "https://localhost:44353/"; // API base URL

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


        public ActionResult Index()
        {
            var functions = new List<RetailerFunction>
            {
                new RetailerFunction { Name = "Add Products", ActionUrl = Url.Action("AddProduct", "Retailer") },
                new RetailerFunction { Name = "View My Products", ActionUrl = Url.Action("MyProducts", "Retailer") }
            };
            return View(functions);
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            PopulateCategories();
            return View(new ProductDTO());
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    product.RetailerID = GetUserId();

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiBaseUrl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var json = JsonConvert.SerializeObject(product);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await client.PostAsync("api/products/add", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = "✅ Product added successfully!";
                            return RedirectToAction("MyProducts");
                        }
                        else
                        {
                            var errorResponse = await response.Content.ReadAsStringAsync();
                            ModelState.AddModelError("", $"⚠ API Error: {response.StatusCode} - {errorResponse}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "⚠ Exception: " + ex.Message);
                }
            }

            PopulateCategories();
            return View(product);
        }

        private void PopulateCategories()
        {
            using (var db = new majorprojectEntities())
            {
                var categories = db.Categories
                    .OrderBy(c => c.CategoryName)
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryID.ToString(),
                        Text = c.CategoryName
                    }).ToList();
                ViewBag.CategoryList = categories;
            }
        }

        // GET: Retailer/EditProduct
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            using (var db = new majorprojectEntities())
            {
                var product = db.Products.FirstOrDefault(p => p.ProductID == id);
                if (product == null)
                    return HttpNotFound();

                return View(product); // 👈 send Product model to view
            }
        }

        // POST: Retailer/EditProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(Product model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var db = new majorprojectEntities())
            {
                var existing = db.Products.FirstOrDefault(p => p.ProductID == model.ProductID);
                if (existing == null)
                    return HttpNotFound();

                // ✅ Update fields
                existing.Name = model.Name;
                existing.Description = model.Description;
                existing.Price = model.Price;
                existing.QuantityAvailable = model.QuantityAvailable;
                existing.RemainingQuantity = model.RemainingQuantity;
                existing.ImageURL = model.ImageURL;

                db.SaveChanges();
            }

            TempData["Message"] = "✅ Product updated successfully!";
            return RedirectToAction("MyProducts");
        }


        // Delete product
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                await client.DeleteAsync($"api/product/delete/{productId}");
            }
            return RedirectToAction("MyProducts");
        }

        // MyProducts
        public async Task<ActionResult> MyProducts()
        {
            int retailerId = GetUserId();
            List<ProductViewDTO> products = new List<ProductViewDTO>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/products/view/retailer/{retailerId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<List<ProductViewDTO>>(json);
                }
            }

            return View(products);
        }
    }
}

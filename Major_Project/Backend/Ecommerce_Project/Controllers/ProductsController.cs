using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Ecommerce_Project.DTO;
using Ecommerce_Project.Models;

namespace Ecommerce_Project.Controllers
{
    public class ProductsController : ApiController
    {
        private majorprojectEntities db = new majorprojectEntities();

        [HttpGet]
        [Route("api/products")]
        public IHttpActionResult GetAllProducts()
        {
            try
            {
                var products = db.Products
                    .Select(p => new ProductDTO
                    {
                        ProductID = p.ProductID,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price ?? 0,                  // in case Price is nullable
                QuantityAvailable = p.QuantityAvailable ?? 0, // in case QuantityAvailable is nullable
                RetailerID = (int)p.RetailerID,             // keep nullable
                CategoryID = (int)p.CategoryID,             // keep nullable
                        RemainingQuantity = p.RemainingQuantity ?? 0,   // safe for NULLs
                        ImageURL = p.ImageURL
                    })
                    .ToList();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/products/{id}
        [HttpGet]
        [Route("api/products/{id}")]
        public IHttpActionResult GetProductById(int id)
        {
            try
            {
                var product = db.Products
                    .Where(p => p.ProductID == id)
                    .Select(p => new ProductDTO
                    {
                        ProductID = p.ProductID,
                        Name = p.Name,
                        Description = p.Description,
                        Price = (decimal)p.Price,
                        QuantityAvailable =(int) p.QuantityAvailable,
                        RetailerID = (int)p.RetailerID,
                        CategoryID = (int)p.CategoryID,
                        ImageURL = p.ImageURL
                    })
                    .FirstOrDefault();

                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/products/add")]
        public IHttpActionResult AddProduct([FromBody] ProductDTO model)
        {
            try
            {
                // Validate required fields
                if (model == null || string.IsNullOrEmpty(model.Name) || model.Price <= 0)
                    return BadRequest("Product name and price are required.");

                // Save product with ImageURL directly
                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    QuantityAvailable = model.QuantityAvailable,
                    RemainingQuantity = model.QuantityAvailable, // ✅ Added line
                    RetailerID = model.RetailerID,
                    CategoryID = model.CategoryID,
                    ImageURL = model.ImageURL, // save URL entered by retailer
                    CreatedAt = DateTime.Now
                };

                db.Products.Add(product);
                db.SaveChanges();

                return Ok(new { Message = "Product added successfully", ProductID = product.ProductID });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/product/edit/{productId}")]
        public IHttpActionResult EditProduct(int productId, [FromBody] ProductViewDTO updatedProduct)
        {
            try
            {
                var product = db.Products.FirstOrDefault(p => p.ProductID == productId);
                if (product == null)
                    return NotFound();

                // Update fields
                product.Name = updatedProduct.Name;
                product.Description = updatedProduct.Description;
                product.Price = updatedProduct.Price;
                product.QuantityAvailable = updatedProduct.QuantityAvailable;
                product.RemainingQuantity = updatedProduct.RemainingQuantity;
                product.ImageURL = updatedProduct.ImageURL;
                product.CreatedAt = updatedProduct.CreatedAt; // optional, or ignore if auto-set

                db.SaveChanges();

                return Ok(new { message = "Product updated successfully." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // =======================
        // DELETE PRODUCT
        // DELETE: api/products/delete/{productId}
        // =======================
        [HttpDelete]
        [Route("api/product/delete/{productId}")]
        public IHttpActionResult DeleteProduct(int productId)
        {
            try
            {
                var product = db.Products.FirstOrDefault(p => p.ProductID == productId);
                if (product == null)
                    return NotFound();

                db.Products.Remove(product);
                db.SaveChanges();

                return Ok(new { message = "Product deleted successfully." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // GET: api/products/view/retailer/{retailerId}
        [HttpGet]
        [Route("api/products/view/retailer/{retailerId}")]
        public IHttpActionResult GetAllRetailerProducts(int retailerId)
        {
            try
            {
                var products = db.Products
                    .Where(p => p.RetailerID == retailerId)
                    .Select(p => new ProductViewDTO
                    {
                        ProductID = p.ProductID,
                        Name = p.Name,
                        Description = p.Description,
                        Price = (int)p.Price,   // no cast needed
                QuantityAvailable = p.QuantityAvailable ?? 0,   // safe for NULLs
                RemainingQuantity = p.RemainingQuantity ?? 0,   // safe for NULLs
                CreatedAt = (DateTime)p.CreatedAt,   // no cast needed
                ImageURL = p.ImageURL
                    })
                    .ToList();

                if (!products.Any())
                    return NotFound();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/products/view/product/{productId}
        [HttpGet]
        [Route("api/products/view/product/{productId}")]
        public IHttpActionResult GetRetailerProductById(int productId)
        {
            try
            {
                var product = db.Products
                    .Where(p => p.ProductID == productId)
                    .Select(p => new ProductViewDTO
                    {
                        ProductID = p.ProductID,
                        Name = p.Name,
                        Description = p.Description,
                        Price = (decimal)p.Price,
                        QuantityAvailable = (int)p.QuantityAvailable,
                        CreatedAt = (DateTime)p.CreatedAt,
                        ImageURL = p.ImageURL
                    })
                    .FirstOrDefault();

                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


    }
}

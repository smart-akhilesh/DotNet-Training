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
    public class CategoriesController : ApiController
    {
        majorprojectEntities db = new majorprojectEntities();

        [HttpGet]
        [Route("api/category/{name}")]
        public IHttpActionResult GetCategoryIdByName(string name)
        {
            try
            {
                //select categories id
                var categoryId = db.Categories
                                   .Where(c => c.CategoryName == name)
                                   .Select(c => c.CategoryID)
                                   .FirstOrDefault();

                if (categoryId == 0)
                    return NotFound();

                //select product on the basis of id
                var product = db.Products
                   .Where(p => p.CategoryID == categoryId)
                     .Select(p => new CategoriesDTO
                     {
                         ProductID = p.ProductID,
                         Name = p.Name,
                         Price = (decimal)p.Price,
                         ImageURL = p.ImageURL

                     }).ToList();

                if (product == null || !product.Any())
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

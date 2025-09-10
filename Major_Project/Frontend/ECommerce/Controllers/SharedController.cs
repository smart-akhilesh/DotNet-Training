using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerce.Models;

namespace ECommerce.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        private readonly majorprojectEntities db = new majorprojectEntities();
        [ChildActionOnly]
        public PartialViewResult CategoryDropdown()
        {
            var categories = db.Categories.OrderBy(c => c.CategoryName).ToList();
            return PartialView("_CategoryDropdown", categories);
        }
    }
}
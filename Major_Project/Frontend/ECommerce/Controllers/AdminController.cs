using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
       

        private readonly majorprojectEntities db = new majorprojectEntities();
            public ActionResult Index()
            {
                var functions = new List<AdminFunction>
                {
                 new AdminFunction { Name = "Register New Admin", ActionUrl = Url.Action("Signup", "Account",new{role="Admin" }) },
                 new AdminFunction { Name = "Manage Users", ActionUrl = Url.Action("ManageUsers", "Admin") },
                 new AdminFunction { Name = "Manage Retailers", ActionUrl = Url.Action("ManageRetailers", "Admin") },
                 new AdminFunction { Name = "Add/Edit Categories", ActionUrl = Url.Action("ManageCategories", "Admin") },
                 new AdminFunction { Name = "View Orders", ActionUrl = Url.Action("ViewOrders", "Admin") },
           
          
                };

                return View(functions);
            }

      

        [HttpGet]
        public ActionResult ManageUsers()
        {
            var users = db.Users.Select(u => new UserViewModel
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                Address = u.Address,
                PhoneNumber = u.PhoneNumber,
                IsActive = u.IsActive
            }).ToList();

            return View(users);
        }

        [HttpPost]
        public ActionResult ManageUsers(string searchTerm)
        {
            var users = db.Users
                .Where(u => u.Username.Contains(searchTerm) || u.Email.Contains(searchTerm))
                .Select(u => new UserViewModel
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    Address = u.Address,
                    PhoneNumber = u.PhoneNumber
                }).ToList();

            return View(users);
        }

        [HttpPost]
        public ActionResult Activation(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                user.IsActive = !user.IsActive; 
                db.SaveChanges();

                string status = user.IsActive ? "activated" : "deactivated";
                TempData["AlertMessage"] = $"{user.Username} has been {status} successfully.";
            }

            return RedirectToAction("ManageUsers");
        }



        [HttpGet]
        public ActionResult ViewUserDetails(int id)
        {
            var user = db.Users
                .Where(u => u.UserId == id)
                .Select(u => new UserViewModel
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    Address = u.Address,
                    PhoneNumber = u.PhoneNumber
                })
                .FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }
        public ActionResult ManageRetailers()
        {
            var retailers = db.Retailers.ToList();
            return View(retailers);
        }

        [HttpPost]
        public ActionResult ManageRetailers(string searchTerm, string approvalFilter)
        {
            var retailers = db.Retailers.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
                retailers = retailers.Where(r => r.Username.Contains(searchTerm) || r.Email.Contains(searchTerm));

            if (!string.IsNullOrEmpty(approvalFilter))
                retailers = retailers.Where(r => r.IsApproved == (approvalFilter == "Approved"));

            return View(retailers.ToList());
        }

        [HttpPost]
        public ActionResult ToggleRetailerApproval(int id)
        {
            var retailer = db.Retailers.Find(id);
            if (retailer != null)
            {
                retailer.IsApproved = !retailer.IsApproved;
                db.SaveChanges();
            }

            return RedirectToAction("ManageRetailers");
        }
        [HttpGet]
        public ActionResult ViewRetailerDetails(int id)
        {
            var retailer = db.Retailers
                .Where(u => u.RetailerId == id)
                .Select(u => new RetailerViewModel
                {
                    RetailerId = u.RetailerId,
                    Username = u.Username,
                    Email = u.Email,
                    Address = u.Address,
                    PhoneNumber = u.PhoneNumber
                })
                .FirstOrDefault();

            if (retailer == null)
            {
                return HttpNotFound();
            }

            return View(retailer);
        }

        [HttpGet]
        public ActionResult ManageCategories(int page = 1, int pageSize = 10)
        {
            var categories = db.Categories
                .OrderBy(c => c.CategoryName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalCategories = db.Categories.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCategories / pageSize);
            ViewBag.CurrentPage = page;

            return View(categories);
        }

        [HttpPost]
        public ActionResult AddCategory(string categoryName)
        {
            string trimmedName = categoryName?.Trim().ToLower();
            bool exists = db.Categories.Any(c => c.CategoryName.ToLower() == trimmedName);

            if (!exists && !string.IsNullOrWhiteSpace(categoryName))
            {
                db.Categories.Add(new Category { CategoryName = categoryName.Trim() });
                db.SaveChanges();
                TempData["Message"] = "Category added successfully!";
            }
            else
            {
                TempData["Error"] = "Category already exists or name is invalid.";
            }

            return RedirectToAction("ManageCategories");
        }

        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(Category model)
        {
            var exists = db.Categories.Any(c => c.CategoryName.ToLower() == model.CategoryName.Trim().ToLower() && c.CategoryID != model.CategoryID);
            if (exists)
            {
                TempData["Error"] = "Another category with the same name exists.";
                return RedirectToAction("ManageCategories");
            }

            var category = db.Categories.Find(model.CategoryID);
            if (category != null)
            {
                category.CategoryName = model.CategoryName.Trim();
                db.SaveChanges();
                TempData["Message"] = "Category updated successfully!";
            }

            return RedirectToAction("ManageCategories");
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            var hasProducts = db.Products.Any(p => p.CategoryID == id);
            if (hasProducts)
            {
                TempData["Error"] = "Cannot delete category. Products are linked to it.";
                return RedirectToAction("ManageCategories");
            }

            var category = db.Categories.Find(id);
            if (category != null)
            {
                db.Categories.Remove(category);
                db.SaveChanges();
                TempData["Message"] = "Category deleted successfully!";
            }

            return RedirectToAction("ManageCategories");
        }
        [HttpGet]

        public ActionResult ViewOrders(string searchUser = "", DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 10)

        {

            var query = from od in db.OrderDetails

                        join o in db.Orders on od.OrderID equals o.OrderID

                        join p in db.Products on od.ProductID equals p.ProductID

                        join u in db.Users on o.UserID equals u.UserId

                        select new OrderDetailViewModel

                        {

                            OrderID = o.OrderID,

                            ProductID = p.ProductID,

                            ProductName = p.Name,

                            Quantity = od.Quantity,

                            PriceAtPurchase = od.PriceAtPurchase,

                            TotalAmount = o.TotalAmount,

                            OrderDate = o.OrderDate,

                            Username = u.Username

                        };

            if (!string.IsNullOrWhiteSpace(searchUser))

                query = query.Where(q => q.Username.Contains(searchUser));

            if (fromDate.HasValue)

                query = query.Where(q => q.OrderDate >= fromDate.Value);

            if (toDate.HasValue)

                query = query.Where(q => q.OrderDate <= toDate.Value);

            int totalRecords = query.Count();

            var pagedOrders = query

                .OrderByDescending(q => q.OrderDate)

                .Skip((page - 1) * pageSize)

                .Take(pageSize)

                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            ViewBag.CurrentPage = page;

            ViewBag.SearchUser = searchUser;

            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");

            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(pagedOrders);

        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ECommerce.Models;

namespace ECommerce.Controllers
{

    public class AccountController : Controller
    {
        majorprojectEntities db = new majorprojectEntities();


        public string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        public ActionResult Signup(string role)
        {
            var model = new RegisterModel
            {
                Role = role 
            };
            ViewBag.Role = role;
            return View(model);
        }

        [HttpPost]

    
        public ActionResult Signup(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var hashedpw = HashPassword(model.Password);
                ViewBag.Message = "Successfully Registered";

                // Normalize role input (optional but safer)
                var role = model.Role?.Trim();

                if (role == "User")
                {
                    db.Users.Add(new User
                    {
                        Username = model.Username,
                        Password = hashedpw,
                        Email = model.Email,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber
                    });

                    Session["Username"] = model.Username;
                    Session["UserRole"] = role;
                }
                else if (role == "Retailer")
                {
                    db.Retailers.Add(new Retailer
                    {
                        Username = model.Username,
                        Password = hashedpw,
                        Email = model.Email,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        IsApproved=false
                       
                    });

                    db.SaveChanges();
                    return RedirectToAction("PendingApproval");
                }
                else if (role == "Admin")
                {
                    db.Admins.Add(new Admin
                    {
                        Username = model.Username,
                        Password = hashedpw,
                        Email = model.Email,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber
                    });
                }
                else
                {
                    ModelState.AddModelError("Role", "Invalid role selected.");
                    return View(model);
                }

                db.SaveChanges();
                return RedirectToAction("Signin", new { role = role });
            }

            // If validation fails, return the view with validation messages
            return View(model);
        }


        public ActionResult Signin(string role)
        {
            ViewBag.Role = role;
            return View();
        }

        [HttpPost]
        public ActionResult Signin(string username, string password, string role)
        {
            role = role?.ToLower();
            int? userId = null;
            bool isValid = false;
            var hashpw = HashPassword(password);

            role = role?.ToLower();

            if (role == "user")
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == hashpw && u.IsActive);
                if (user != null)
                {
                    isValid = true;
                    userId = user.UserId;
                }
            }
            else if (role == "retailer")
            {
                var retailer = db.Retailers.FirstOrDefault(r => r.Username == username && r.Password == hashpw);
                if (retailer != null)
                {
                    if (!retailer.IsApproved)
                    {
                        return RedirectToAction("WaitingForApproval");
                    }
                    isValid = true;
                    userId = retailer.RetailerId;

                }
            }
            else if (role == "admin")
            {
                var admin = db.Admins.FirstOrDefault(a => a.Username == username && a.Password == hashpw);
                if (admin != null)
                {
                    isValid = true;
                    userId = admin.AdminId;
                }
            }

            if (isValid && userId.HasValue)
            {
                Session["UserID"] = userId.Value;   
                Session["Username"] = username;
                Session["UserRole"] = role;

                FormsAuthentication.SetAuthCookie(username, false);

                // Redirect to respective dashboard based on role
                return RedirectToAction("Index", role.First().ToString().ToUpper() + role.Substring(1));
            }
            else
            {
                ViewBag.Role = role;
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "User");
        }


        [HttpGet]
        public ActionResult ForgotPassword(string role)
        {
            ViewBag.Role = role;
            return View(new ForgotPassword());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassword model, string role)
        {
            if (ModelState.IsValid)
            {

                string confirmPassword = Request["ConfirmPassword"];
                if (model.Password != confirmPassword)
                {
                    ModelState.AddModelError("", "Passwords do not match.");
                    ViewBag.Role = role;
                    return View(model);
                }

                role = role?.ToLower();
                bool updated = false;

                if (role == "user")
                {
                    var user = db.Users.FirstOrDefault(u => u.Username == model.Username);
                    if (user != null)
                    {
                        user.Password = HashPassword(model.Password);
                        db.SaveChanges();
                        updated = true;
                    }
                }
                else if (role == "admin")
                {
                    var admin = db.Admins.FirstOrDefault(a => a.Username == model.Username);
                    if (admin != null)
                    {
                        admin.Password = HashPassword(model.Password);
                        db.SaveChanges();
                        updated = true;
                    }
                }
                else if (role == "retailer")
                {
                    var retailer = db.Retailers.FirstOrDefault(r => r.Username == model.Username);
                    if (retailer != null)
                    {
                        retailer.Password = HashPassword(model.Password);
                        db.SaveChanges();
                        updated = true;
                    }
                }

                if (updated)
                {
                    return RedirectToAction("Signin", new { role = role });
                }


                ModelState.AddModelError("", "User not found or role is invalid.");
            }
            ViewBag.Role = role;
            return View(model);
        }



        [HttpGet]
        public ActionResult EditProfile()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Signin", "Account");
            }

            string username = Session["Username"].ToString();
            string role = Session["UserRole"]?.ToString();

            RegisterModel model = null;

            if (role == "user")
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    model = new RegisterModel
                    {
                        Username = user.Username,
                       
                        Email = user.Email,
                        Address = user.Address,
                        PhoneNumber = user.PhoneNumber,
                    };
                }
            }
            else if (role == "retailer")
            {
                var retailer = db.Retailers.FirstOrDefault(r => r.Username == username);
                if (retailer != null)
                {
                    model = new RegisterModel
                    {
                        Username = retailer.Username,
                     
                        Email = retailer.Email,
                        Address = retailer.Address,
                        PhoneNumber = retailer.PhoneNumber,
                    };
                }
            }
            else if (role == "admin")
            {
                var admin = db.Admins.FirstOrDefault(a => a.Username == username);
                if (admin != null)
                {
                    model = new RegisterModel
                    {
                        Username = admin.Username,
                      
                        Email = admin.Email,
                        Address = admin.Address,
                        PhoneNumber = admin.PhoneNumber,
                    };
                }
            }

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditProfile(RegisterModel model)
        {
            ModelState.Remove("Password");
            string username = Session["Username"].ToString();
            string role = Session["UserRole"]?.ToString().ToLower();
            bool updated = false;
            if (ModelState.IsValid && username!=null && role!=null)
            {

                if (role == "user")
                {
                    var user = db.Users.FirstOrDefault(u => u.Username == username);
                    if (user != null)
                    {
                        user.Email = model.Email;
                        user.Address = model.Address;
                        user.PhoneNumber = model.PhoneNumber;
                        updated = true;
                    }
                }
                else if (role == "retailer")
                {
                    var retailer = db.Retailers.FirstOrDefault(r => r.Username == username);
                    if (retailer != null)
                    {
                        retailer.Email = model.Email;
                        retailer.Address = model.Address;
                        retailer.PhoneNumber = model.PhoneNumber;
                        updated = true;
                    }
                }
                else if (role == "admin")
                {
                    var admin = db.Admins.FirstOrDefault(a => a.Username == username);
                    if (admin != null)
                    {
                        admin.Email = model.Email;
                        admin.Address = model.Address;
                        admin.PhoneNumber = model.PhoneNumber;
                        updated = true;
                    }
                }

                if (updated)
                {
                    db.SaveChanges();
                    ViewBag.Message = "Profile updated successfully!";
                }
                else
                {
                    ViewBag.Message = "Update failed. User not found.";
                }
            }
            else
            {
                ViewBag.Message = "Invalid Input";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAccount()
        {
            string username = Session["Username"]?.ToString();
            string role = Session["UserRole"]?.ToString()?.ToLower();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(role))
            {
                return RedirectToAction("Signin", "Account");
            }

            if (role == "user")
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    user.IsActive = false;
                    db.SaveChanges();

                    TempData["AlertMessage"] = "Your account has been deleted successfully.";
                    return RedirectToAction("Logout");
                }
            }

            return RedirectToAction("Logout");
        }

        public ActionResult PendingApproval()
        {
            return View();
        }

        public ActionResult WaitingForApproval()
        {
            return View();
        }
    }
}
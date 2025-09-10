using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerce.Models;
namespace ECommerce.Controllers
{
    public class UserController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }
    }
}
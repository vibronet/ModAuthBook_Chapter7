using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace WebAppChapter7.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            string userfirstname = ClaimsPrincipal.Current.FindFirst(ClaimTypes.GivenName).Value;
            ViewBag.Message = String.Format("Welcome, {0}!", userfirstname);

            return View();
        }
    }
}
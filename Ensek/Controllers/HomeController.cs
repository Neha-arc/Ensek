using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ensek.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MeterReading()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Customer()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]

        public ActionResult Upload(HttpPostedFileBase upload)
        {
            return Content("Ok");
        }
    }
}
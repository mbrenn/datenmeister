using DatenMeisterWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DatenMeisterWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Index(UserLoginModel model)
        {
            if (this.ModelState.IsValid)
            {
                if (model.username == "mbrenn" && model.password == "abc")
                {
                    return this.RedirectToAction("Index", "Extents");
                }
            }

            return this.View();
        }
    }
}
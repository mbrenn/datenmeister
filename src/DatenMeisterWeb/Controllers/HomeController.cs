using DatenMeister;
using DatenMeister.Transformations;
using DatenMeister.Pool;
using DatenMeister.Web;
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
        /// <summary>
        /// Stores the server manager
        /// </summary>
        private IServerManager serverManager;

        public HomeController(IServerManager serverManager)
        {
            this.serverManager = serverManager;
        }

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
                // Get users
                var userManagementExtent = this.serverManager.GetServerPool().GetExtentByUri(
                    ServerManager.UriUserManagement);
                var foundUser = userManagementExtent.Elements()
                    .FilterByProperty("username", model.username)
                    .FirstOrDefault()
                    .AsIObjectOrNull();
                if (foundUser != null)
                {
                    if ( foundUser.getAsSingle("password").ToString() == model.password)
                    {
                        return this.RedirectToAction("Index", "Extents");
                    }
                }
            }

            return this.View();
        }
    }
}
using DatenMeister.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DatenMeisterWeb.Controllers
{
    public class ExtentsController : Controller
    {
        /// <summary>
        /// Stores the server manager
        /// </summary>
        private IServerManager serverManager;

        public ExtentsController(IServerManager serverManager)
        {
            this.serverManager = serverManager;
        }

        public ActionResult Index()
        {
            return this.View();
        }
    }
}
using DatenMeister;
using DatenMeister.Logic;
using DatenMeister.Logic.Views;
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
            var dataPool = this.serverManager.GetDataPool();

            return this.View(dataPool.ExtentContainer);
        }

        public ActionResult Show(string uri)
        {
            var dataPool = this.serverManager.GetDataPool();
            var foundExtent = dataPool.ExtentContainer
                .Where(x => x.Extent.ContextURI() == uri)
                .FirstOrDefault();

            if (foundExtent == null)
            {
                // No extent found...
                return this.RedirectToAction("Index");
            }

            // Found an extent
            var elements = foundExtent.Extent.Elements();
            return this.View(
                new MVVMExtentOverview()
                {
                    Extent = foundExtent.Extent,
                    Elements = elements,
                    Columns = ReflectiveSequenceHelper.GetConsolidatedPropertyNames(elements)
                });
        }

        public class MVVMExtentOverview
        {
            public IURIExtent Extent
            {
                get;
                set;
            }

            public IReflectiveSequence Elements
            {
                get;
                set;
            }

            public IEnumerable<string> Columns
            {
                get;
                set;
            }
        }
    }
}
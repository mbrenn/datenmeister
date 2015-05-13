using DatenMeister;
using DatenMeister.Logic;
using DatenMeister.Web;
using DatenMeisterWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DatenMeisterWeb.API
{
    public class ExtentController : ApiController
    {
        /// <summary>
        /// Stores the server manager
        /// </summary>
        private IServerManager serverManager;

        public ExtentController(IServerManager serverManager)
        {
            this.serverManager = serverManager;
        }

        public object Get(string uri)
        {
            var dataPool = this.serverManager.GetDataPool();
            var foundExtent = dataPool.ExtentContainer
                .Where(x => x.Extent.ContextURI() == uri)
                .FirstOrDefault();

            if (foundExtent == null)
            {
                // No extent found...
                return null;
            }

            // Found an extent
            var elements = foundExtent.Extent.Elements();
            var columns = ReflectiveSequenceHelper.GetConsolidatedPropertyNames(elements);

            List<object> values = new List<object>();
            foreach (var element in elements.Select(x => x.AsIObject()))
            {
                var value = new Dictionary<string, object>();
                foreach (var column in columns)
                {
                    if (element.isSet(column))
                    {
                        value[column] = element.get(column).ToString();
                    }
                }

                values.Add(value);
            }

            return
                new
                {
                    ExtentUri = foundExtent.Extent.ContextURI(),
                    Elements = values,
                    Columns = ReflectiveSequenceHelper.GetConsolidatedPropertyNames(elements)
                };
        }
    }
}

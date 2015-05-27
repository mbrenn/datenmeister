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

        /// <summary>
        /// Gets the complete extent by the uri of an extet
        /// </summary>
        /// <param name="uri">Uri to be queried</param>
        /// <returns>The Json object being sent to the browser</returns>
        [HttpGet]
        public object ExtentByUri(string uri)
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
                var data = new Dictionary<string, object>();
                foreach (var column in columns)
                {
                    if (element.isSet(column))
                    {
                        data[column] = element.get(column).ToString();
                    }
                }

                var id = element.Id;
                values.Add(new
                {
                    id = id,
                    data = data
                });
            }

            return
                new
                {
                    extentUri = foundExtent.Extent.ContextURI(),
                    elements = values,
                    columns = ReflectiveSequenceHelper.GetConsolidatedPropertyNames(elements)
                };
        }

        public class DetailParam
        {
            public string uri { get; set; }
            public string id { get; set; }
        }

        [HttpGet]
        public object Detail(string uri, string objectId)
        {
            var objectToBeQueried = this.GetObjectByUriAndId(uri, objectId);
            if (objectToBeQueried == null)
            {
                return null;
            }

            var rows = new List<string>();
            var data = new Dictionary<string, string>();
            foreach (var property in objectToBeQueried.getAll())
            {
                rows.Add(property.PropertyName);
                data[property.PropertyName] = property.Value.ToString();
            }

            return new
            {
                rows = rows,
                data = data
            };
        }

        public class DeleteParam
        {
            public string uri { get; set; }
            public string objectId { get; set; }
        }

        [HttpPost]
        public void Delete(DeleteParam param)
        {
            var uri = param.uri;
            var id = param.objectId;

            var objectToBeDeleted = this.GetObjectByUriAndId(param.uri, param.objectId);
            if (objectToBeDeleted != null)
            {
                objectToBeDeleted.delete();
            }
        }

        /// <summary>
        /// Gets the object by using a uri and an id
        /// </summary>
        /// <param name="uri">Uri, which is questioned</param>
        /// <param name="id">Id to be queried</param>
        /// <returns>Retrieved object</returns>
        private IObject GetObjectByUriAndId(string uri, string id)
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

            // Gets the object that shall be deleted
            var objectToBeDeleted =
                foundExtent.Extent.Elements()
                .Select(x => x.AsIObject())
                .Where(x => x.Id == id)
                .FirstOrDefault();
            return objectToBeDeleted;
        }
    }
}

﻿using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Web
{
    public class ExtentController : Controller
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(ExtentController));

        [Inject]
        public DatenMeisterPool Pool
        {
            get;
            set;
        }

        [WebMethod]
        public IActionResult GetServerInfo()
        {
            return this.Json(new ServerInfo()
            {
                serverAddress = "http://localhost:8081",
                serverInfo = "DatenMeister Demoserver",
                success = true
            });
        }

        [WebMethod]
        public IActionResult GetExtentInfos()
        {
            var extents = this.Pool.Extents.Select(x =>
                 new
                 {
                     uri = x.ContextURI(),
                     type = x.GetType().FullName
                 });

            return this.Json(new
            {
                success = true,
                extents = extents
            });
        }

        [WebMethod]
        public IActionResult GetObjectsInExtent(string uri)
        {
            var extent = this.Pool.Extents.Where(x => x.ContextURI() == uri).FirstOrDefault();
            if (extent == null)
            {
                throw new MVCProcessException("uri_not_found", "URI has not been found");
            }

            var data = new JsonExtentData();
            
            var elements = extent.Elements();
            var titles = elements.GetColumnTitles();
            data.columns.AddRange(titles.Select(x => new JsonExtentColumnInfo()
            {
                name = x
            }));

            foreach (var element in elements)
            {
                var dict = new Dictionary<string, string>();
                foreach (var pair in element.GetAll())
                {
                    dict[pair.PropertyName] = pair.Value.ToString();
                }

                data.objects.Add(new JsonExtentObject(element.Id, dict));
            }

            return this.Json(data);
        }

        /// <summary>
        /// Deletes an object from extent
        /// </summary>
        /// <param name="uri">Uri to be deleted</param>
        /// <returns>Action result </returns>
        [WebMethod]
        public IActionResult DeleteObject(string uri)
        {
            IURIExtent extent;
            var element = GetElementByUri(uri, out extent);

            logger.Message("Item shall be deleted: " + element.Id);
            element.Delete();

            return this.SuccessJson();
        }


        [WebMethod]
        public IActionResult EditObject(string uri, [PostModel] Dictionary<string, string> values)
        {
            IURIExtent extent;
            var element = this.GetElementByUri(uri, out extent);
            foreach (var pair in values)
            {
                element.Set(pair.Key, pair.Value);
            }

            return this.SuccessJson();
        }

        /// <summary>
        /// Gets an element by the uri of the element
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="extent"></param>
        /// <returns></returns>
        private IObject GetElementByUri(string uri, out IURIExtent extent)
        {
            var positionHash = uri.IndexOf('#');
            if (positionHash == -1)
            {
                throw new MVCProcessException("invalid_url", "Hash ('#') is not given");
            }

            var extentUri = uri.Substring(0, positionHash);
            var objectId = uri.Substring(positionHash + 1); ;
            extent = this.Pool.Extents.Where(x => x.ContextURI() == extentUri).FirstOrDefault();
            if (extent == null)
            {
                throw new MVCProcessException("uri_not_found", "URI has not been found");
            }

            var element = extent.Elements().Where(x => x.Id == objectId).FirstOrDefault();
            if (element == null)
            {
                throw new MVCProcessException("object_not_found", "Object has not been found");
            }

            return element;
        }
    }
}

using BurnSystems.Logging;
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
            var uriObject = new Uri(uri);

            var extentUri = uriObject.AbsolutePath;
            var objectId = uriObject.Fragment;
            var extent = this.Pool.Extents.Where(x => x.ContextURI() == extentUri).FirstOrDefault();
            if (extent == null)
            {
                throw new MVCProcessException("uri_not_found", "URI has not been found");
            }

            var element = extent.Elements().Where(x => x.Id == objectId).FirstOrDefault();
            if (element == null)
            {
                throw new MVCProcessException("object_not_found", "Object has not been found");
            }

            logger.Message("Item shall be deleted: " + uri);

            return this.SuccessJson();
        }
    }
}

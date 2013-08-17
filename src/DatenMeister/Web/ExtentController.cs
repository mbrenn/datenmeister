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

            var data = new ExtentData();
            
            var elements = extent.Elements();
            var titles = elements.GetColumnTitles();
            data.columns.AddRange(titles.Select(x => new ExtentColumnInfo()
            {
                name = x
            }));

            foreach (var element in elements)
            {
                var dict = new Dictionary<string, string>();
                foreach (var pair in element.GetAll())
                {
                    dict[pair.First] = pair.Second.ToString();
                }

                data.objects.Add(dict);
            }

            return this.Json(data);
        }
    }
}

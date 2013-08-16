using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
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
        public IActionResult GetObjectsInExtent(string url)
        {
            var extent = this.Pool.Extents.Where(x => x.ContextURI() == url).FirstOrDefault();
            if (extent == null)
            {

            }

            throw new InvalidOperationException();
        }
    }
}

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
            throw new InvalidOperationException();
        }

        [WebMethod]
        public IActionResult GetObjectsInExtent(string url)
        {
            throw new InvalidOperationException();
        }
    }
}

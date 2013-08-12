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
            throw new InvalidOperationException();
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

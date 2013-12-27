using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.WebServer.Modules.MVC;

namespace BurnSystems.WebServer.Responses.Tests
{
    public class TestController : Controller
    {
        [WebMethod]
        public IActionResult Today()
        {
            return this.Html(DateTime.Now.ToString());
        }

        [WebMethod]
        public IActionResult Test()
        {
            return this.Html("Test");
        }

        [WebMethod]
        public IActionResult Greet(string name)
        {
            return this.Html("Hello " + name);
        }

        [WebMethod]
        public IActionResult Add(int a, int b)
        {
            return this.Html(string.Format("{0} + {1} = {2}", a, b, a + b));
        }

        [WebMethod(Name = "Subtract")]
        public IActionResult Minus(int a, int b)
        {
            return this.Html(string.Format("{0} - {1} = {2}", a, b, a - b));
        }
    }
}

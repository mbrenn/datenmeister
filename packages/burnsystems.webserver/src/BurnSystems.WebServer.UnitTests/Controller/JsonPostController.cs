using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.UnitTests.Controller
{
    public class JsonPostController: Modules.MVC.Controller
    {
        [WebMethod]
		public IActionResult Load (string uri, [PostModel] JsonPostTestStructure structure)
		{
			Console.WriteLine (uri);
            return this.Html("Uri: " + uri + "\r\nProperty: " + structure.prop + "\r\nName: " + structure.substructure.name);
        }
    }
}

using BurnSystems.WebServer.Modules.MVC;
using DatenMeister.Logic.ClientActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Web
{
    public static class Extensions
    {
        public static IActionResult ReturnClientAction(
            this Controller controller,
            params IClientAction[] clientAction)
        {
            throw new NotImplementedException();
        }
    }
}

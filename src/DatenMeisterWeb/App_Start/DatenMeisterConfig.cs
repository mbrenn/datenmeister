using DatenMeister.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DatenMeisterWeb.App_Start
{
    public class DatenMeisterConfig
    {
        public static void StartUp()
        {
            var serverManager = DependencyResolver.Current.GetService<IServerManager>(); ;
            serverManager.Init();
        }
    }
}
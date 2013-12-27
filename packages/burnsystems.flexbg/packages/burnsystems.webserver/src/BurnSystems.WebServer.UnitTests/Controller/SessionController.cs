using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Parser;
using BurnSystems.WebServer.Modules.Sessions;
using BurnSystems.WebServer.Modules.MVC;

namespace BurnSystems.WebServer.UnitTests.Controller
{
    /// <summary>
    /// Defines the session controller 
    /// </summary>
    public class SessionController : Modules.MVC.Controller
    {
        [Inject]
        public Session Session
        {
            get;
            set;
        }

        [Inject]
        public ITemplateParser TemplateParser
        {
            get;
            set;
        }

        [WebMethod]
        public IActionResult SessionTest([PostModel] SessionPostModel postModel, [Inject("PageTemplate")] string template)
        {
            if (postModel != null)
            {
                this.Session["Test"] = postModel.Value;
            }

            var model = new
            {
                SessionValue = this.Session["Test"]
            };

            return this.Html(this.TemplateParser.Parse(template, model, null));
        }
        
        /// <summary>
        /// Post Model
        /// </summary>
        public class SessionPostModel
        {
            public string Value
            {
                get;
                set;
            }
        }
    }
}

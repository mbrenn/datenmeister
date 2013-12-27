using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Parser;
using BurnSystems.WebServer.Modules.MVC;

namespace BurnSystems.WebServer.UnitTests.Controller
{
    /// <summary>
    /// Demo controller for Post
    /// </summary>
    public class PostController : Modules.MVC.Controller
    {        
        public class PostTestModel
        {
            public string Prename
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }
        }

        [Inject]
        public ITemplateParser TemplateParser
        {
            get;
            set;
        }

        [WebMethod]
        public IActionResult PostTest([PostModel] PostTestModel model)
        {
            return this.TemplateOrJson(model);
        }
    }
}

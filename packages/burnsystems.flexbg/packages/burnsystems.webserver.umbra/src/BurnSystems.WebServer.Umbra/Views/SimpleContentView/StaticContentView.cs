using BurnSystems.WebServer.Umbra.Requests;
using BurnSystems.WebServer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.SimpleContentView
{
    public class StaticContentView : BaseUmbraRequest
    {
        public string Headline
        {
            get;
            set;
        }

        public StaticContentView(string title, string headline, string content)
        {
            this.Headline = headline;
            this.Title = title;
            this.Content = content;
        }

        /// <summary>
        /// Returns a specific content and shows it on screen. 
        /// Can be used for errors and other stuff
        /// </summary>
        /// <param name="container"></param>
        /// <param name="context"></param>
        public override void Dispatch(ObjectActivation.IActivates container, Dispatcher.ContextDispatchInformation context)
        {
            context.Context.DisableBrowserCache();

            this.ViewTypeToken = "BurnSystems.Umbra.StaticContentView";
            this.UserData = new
            {
                headline = this.Headline,
                title = this.Title,
                content = this.Content
            };

            this.AddScript(
                "js/viewtypes/umbra.viewtypes.staticcontentview.js");
        }
    }
}

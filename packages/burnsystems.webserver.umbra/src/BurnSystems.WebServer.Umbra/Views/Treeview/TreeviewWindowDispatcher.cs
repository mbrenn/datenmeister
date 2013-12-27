using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Umbra.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.Treeview
{
    public class TreeviewWindowDispatcher : BaseUmbraRequest
    {
        /// <summary>
        /// Gets or sets the entities, where the data can be retrieved
        /// </summary>
        public string EntityUrl
        {
            get;
            set;
        }

        public TreeviewWindowDispatcher(Func<ContextDispatchInformation, bool> filter)
            : base(filter)
        {
        }

        public TreeviewWindowDispatcher(Func<ContextDispatchInformation, bool> filter, string entityUrl)
            : base(filter)
        {
            this.EntityUrl = entityUrl;
        }

        /// <summary>
        /// Performs the dispatch
        /// </summary>
        /// <param name="container">Container to be used</param>
        /// <param name="context">Context to be used</param>
        public override void Dispatch(IActivates container, Dispatcher.ContextDispatchInformation context)
        {
            this.Content = "THIS IS CONTENT!";
            this.ViewTypeToken = "BurnSystems.Umbra.TreeView";
            this.AddScript(
                "js/lib/viewtypes/umbra.viewtypes.treeview.js");
            this.Title = "Navigation";

            this.UserData = new
            {
                entityUrl = this.EntityUrl
            };
        }
    }
}

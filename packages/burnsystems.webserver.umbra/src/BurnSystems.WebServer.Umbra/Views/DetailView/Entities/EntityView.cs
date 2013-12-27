using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.PostVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.DetailView.Entities
{
    /// <summary>
    /// Implements the entity view being used to show the contents of a specific object
    /// </summary>
    public class EntityView : DetailView
    {
        /// <summary>
        /// Gets or sets  the configurations
        /// </summary>
        public EntityViewConfig Config
        {
            get;
            set;
        }


        /// <summary>
        /// Initializes a new instance of the EntityView class
        /// </summary>
        /// <param name="config">Config to be used for this instance</param>
        public EntityView(EntityViewConfig config)
        {
            this.Config = config;
        }

        public override void Dispatch(IActivates container, Dispatcher.ContextDispatchInformation context)
        {
            var type = context.Context.Request.QueryString["t"] ?? "show";

            if (type == "show")
            {
                this.AddScript(
                    "viewtypes/umbra.viewtypes.entityview");
                this.Title = this.Item.ToString();
                this.ViewTypeToken = "BurnSystems.Umbra.DetailView.EntityView";

                var viewData = new
                {
                    tables = this.Config.Tables.Select(x => x.ToJson(container, this.Item)),
                    updateUrl = context.Context.Request.Url.ToString()
                };

                this.UserData = viewData;
            }
            else if (type == "update")
            {
                var n = context.Context.Request.QueryString["n"];
                var postVariables = container.Get<PostVariableReader>();
                foreach (var pair in postVariables)
                {
                    var element = this.Config
                        .DetailTables.Where(x=> x.Name == n) // Find table
                        .SelectMany (y => y.Elements)        // Go through all elements fo table
                        .Where(x => x.Name == pair.Key)      // Where we found a match
                        .FirstOrDefault();

                    if (element == null)
                    {
                        continue;
                    }

                    element.SetValue(this.Item.Entity, pair.Value);
                }

                this.Item.ApplyChanges(container);
            }
        }
    }
}

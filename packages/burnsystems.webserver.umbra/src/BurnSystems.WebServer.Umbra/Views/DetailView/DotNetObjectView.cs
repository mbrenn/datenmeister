using BurnSystems.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.DetailView
{
    /// <summary>
    /// Returns the properties of the object
    /// </summary>
    public  class DotNetObjectView : DetailView
    {
        public override void Dispatch(ObjectActivation.IActivates container, Dispatcher.ContextDispatchInformation context)
        {
            this.AddScript(
                "js/lib/viewtypes/umbra.viewtypes.dotnetobjectview.js");
            this.Title = this.Item.ToString();
            this.ViewTypeToken = "BurnSystems.Umbra.DetailView.DotNetObjectView";

            // Converts item to object
            var result = new List<object>();
            foreach (var property in this.Item.GetFieldValues())
            {
                var item = new
                {
                    name = property.Name,
                    value = property.Value
                };

                result.Add(item);
            }

            // Result
            this.UserData = new
            {
                properties = result, 
                title = this.Item.ToString(),
                type = this.Item.GetType().ToString()
            };
                
        }
    }
}

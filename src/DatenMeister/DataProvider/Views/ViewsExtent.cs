using DatenMeister.DataProvider.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Views
{
    /// <summary>
    /// Defines the view extent, which stores the default important view objects as preinitialized .Net-Objects. 
    /// </summary>
    public class ViewsExtent : DotNetExtent
    {
        public ViewsExtent(string extentUri)
            : base(extentUri)
        {
        }

        /// <summary>
        /// Fills the defaultview with items
        /// </summary>
        public void Fill()
        {
            var view = new View();
            view.name = "Views.DatenMeister.View";
            view.fieldInfos.Add(
                new FieldInfo()
                {
                    title = "Name of view",
                    name = "name"
                });

            this.Elements().add(view);
        }
    }
}

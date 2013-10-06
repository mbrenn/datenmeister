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
            : base ( extentUri )
        {
        }

        public void Fill()
        {
            var view = new View();
            view.title = "View for views";
            view.fieldInfos.Add(
                new FieldInfo()
                {
                    title = "Title",
                    name = "title"
                });

            this.Add(view);
        }
    }
}

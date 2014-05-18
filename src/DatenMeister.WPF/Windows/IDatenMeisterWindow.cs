
using DatenMeister.DataProvider.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Defines all the methods that are implemented by the DatenMeister
    /// </summary>
    public interface IDatenMeisterWindow
    {
        /// <summary>
        /// Gets or sets the datenmeister settings
        /// </summary>
        IDatenMeisterSettings Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the title of the application
        /// </summary>
        /// <param name="title">Title of the application</param>
        void SetTitle(string title);

        /// <summary>
        /// Adds an extent view by using the ViewInformation
        /// </summary>
        /// <param name="fieldInfo">The information field</param>
        void AddExtentView(IObject fieldInfo);
    }
}

using DatenMeister.DataProvider;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Logic;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DatenMeister.WPF.Helper
{
    /// <summary>
    /// Defines some menu helper, which can be used for some default entries
    /// </summary>
    public static class MenuHelper
    {
        /// <summary>
        /// Adds the view, that the user can be 
        /// </summary>
        /// <param name="window">Window which sall be used</param>
        public static void AddExtentView(IDatenMeisterWindow window, IURIExtent extentView)
        {
            var menuItem = new MenuItem();
            menuItem.Header = Localization_DatenMeister_WPF.Menu_ViewExtents;
            menuItem.Click += (x, y) =>
                {
                    // Creates the view for the extents
                    var newView = DatenMeisterPoolExtent.AddView(extentView);
                    window.RefreshViews();

                    window.AssociateDetailOpenEvent(newView, (z) =>
                    {
                        MessageBox.Show("Something will happen");
                    });
                };

            window.AddMenuEntry(Localization_DatenMeister_WPF.Menu_View, menuItem);
        }
    }
}

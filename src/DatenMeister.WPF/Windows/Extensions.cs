using DatenMeister.WPF.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Contains several extension methods for windows
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates the window for the DatenMeister. 
        /// </summary>
        /// <param name="core">Application of the window</param>
        /// <returns>Returned window</returns>
        public static IDatenMeisterWindow CreateWindow(this ApplicationCore core)
        {
            var wnd = new DatenMeisterWindow(core);

            // Just sets the title and shows the Window
            wnd.Show();
            wnd.RefreshTabs();

            wnd.AssociateDetailOpenEvent(core.ViewRecentObjects, (x) =>
            {
                var filePath = DatenMeister.Entities.AsObject.DM.RecentProject.getFilePath(x.Value);
                if (File.Exists(filePath))
                {
                    wnd.LoadAndOpenFile(filePath);
                    wnd.Settings.ViewExtent.Elements().remove(core.ViewRecentObjects);
                    wnd.RefreshTabs();
                }
                else
                {
                    MessageBox.Show(Localization_DatenMeister_WPF.Open_FileDoesNotExist);
                }
            });

            return wnd;
        }
    }
}

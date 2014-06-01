using DatenMeister.Logic.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            wnd.RefreshTab();

            return wnd;
        }
    }
}

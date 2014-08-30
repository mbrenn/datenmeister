using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatenMeister.WPF.Windows
{
    public class WindowFactory
    {
        /// <summary>
        /// Creates the window for the DatenMeister. 
        /// </summary>
        /// <param name="core">Application of the window</param>
        /// <returns>Returned window</returns>
        public static IDatenMeisterWindow CreateWindow(ApplicationCore core)
        {
            var wnd = new DatenMeisterWindow(core);

            // Just sets the title and shows the Window
            wnd.LoadExampleData();
            wnd.Show();
            wnd.RegisterToChangeEvent();
            wnd.RefreshTabs();

            return wnd;
        }

        public static void AutosetWindowSize(Window wnd, double ratio = 1.0)
        {
            var width = wnd.Width;
            var height = wnd.Height;

            var newWidth = System.Windows.SystemParameters.PrimaryScreenWidth / 2 * ratio;
            var newHeight = System.Windows.SystemParameters.PrimaryScreenHeight / 2 * ratio;

            wnd.Left -= newWidth / 2;
            wnd.Top -= newHeight / 2;
            wnd.Width = newWidth;
            wnd.Height = newHeight;
        }
    }
}

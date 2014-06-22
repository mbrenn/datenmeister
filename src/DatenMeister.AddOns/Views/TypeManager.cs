using BurnSystems.Test;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DatenMeister.AddOns.Views
{
    /// <summary>
    /// Integrates the type manager into the DatenMeister
    /// </summary>
    public static class TypeManager
    {
        /// <summary>
        /// Integrates the type manager into DatenMeister
        /// </summary>
        /// <param name="window">Window, where the Type Manager will be integrated</param>
        public static void Integrate(IDatenMeisterWindow window)
        {
            var menuItem = new MenuItem();
            menuItem.Header = Localization_DM_Addons.Menu_TypeManager;
            menuItem.Click += (x, y) =>
                {
                    Ensure.That(window.Settings.TypeExtent != null, "No Type extent has been defined");

                };

            window.AddMenuEntry(Localization_DM_Addons.Menu_Views, menuItem);
        }
    }
}

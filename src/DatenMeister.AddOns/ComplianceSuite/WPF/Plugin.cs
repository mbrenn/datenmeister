using DatenMeister.DataProvider;
using DatenMeister.WPF.Controls;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon;

namespace DatenMeister.AddOns.ComplianceSuite.WPF
{
    /// <summary>
    /// Defines the Compliance Suite Plugin
    /// </summary>
    public class Plugin
    {
        public static void Integrate(IDatenMeisterWindow window)
        {
            var menuItem = new RibbonButton();
            menuItem.Label = Localization_DM_Addons.Menu_ComplianceSuite;
            menuItem.LargeImageSource = AddOnHelper.LoadIcon("x-office-document.png");
            menuItem.Click += (x, y) =>
                {
                    // Test for Generic Extent
                    Func<IURIExtent> factoryGeneric = () => new GenericExtent("datenmeister:///temp");
                    Func<IObject> factoryObject = () => new GenericObject();
                    var suite = new ComplianceSuite.Suite(factoryGeneric, factoryObject);
                    var result = suite.Run();

                    DetailDialog.ShowDialogFor(
                        result,
                        window.Settings,
                        null,
                        true);
                };

            window.AddMenuEntry(
                Localization_DM_Addons.Menu_Report,
                menuItem);
        }        
    }
}

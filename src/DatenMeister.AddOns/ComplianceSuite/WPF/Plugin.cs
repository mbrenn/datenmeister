using DatenMeister.DataProvider;
using DatenMeister.WPF.Controls;
using DatenMeister.WPF.Modules.IconRepository;
using DatenMeister.WPF.Windows;
using Ninject;
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
            menuItem.LargeImageSource = Injection.Application.Get<IIconRepository>().GetIcon("x-office-document.png");
            menuItem.Click += (x, y) =>
                {
                    var dialog = new SelectSuite();
                    if (dialog.ShowDialog() == true)
                    {
                        var suite = dialog.GetSuite();
                        if (suite == null)
                        {
                            return;
                        }

                        var result = suite.Run();

                        DetailDialog.ShowDialogFor(
                            result,
                            null,
                            true);
                    }
                };

            window.AddMenuEntry(
                Localization_DM_Addons.Menu_Report,
                menuItem);
        }        
    }
}

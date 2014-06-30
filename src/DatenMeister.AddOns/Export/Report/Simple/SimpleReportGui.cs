using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;

namespace DatenMeister.AddOns.Export.Report.Simple
{
    /// <summary>
    /// Includes a simple gui to export an extent
    /// </summary>
    public class SimpleReportGui
    {
        public static void Integrate(IDatenMeisterWindow window)
        {
            var menuItem = new RibbonButton();
            menuItem.Label = Localization_DM_Addons.Menu_SimpleReport;
            menuItem.LargeImageSource = AddOnHelper.LoadIcon("x-office-document.png");
            menuItem.Click += (x, y) =>
                {
                    var gui = new SimpleReportGui();
                    gui.StartExport(window);
                };

            window.AddMenuEntry(
                Localization_DM_Addons.Menu_Report,
                menuItem);
        }

        /// <summary>
        /// Starts the export
        /// </summary>
        public void StartExport(IDatenMeisterWindow window)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = Localization_DM_Addons.Filter_HtmlExport;
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == true)
            {
                var settings = new SimpleReportSettings();
                var export = new SimpleReport();
                export.Export(
                    window.Settings.ProjectExtent.AsReflectiveCollection(),
                    dlg.FileName,
                    settings);
            }
        }
    }
}

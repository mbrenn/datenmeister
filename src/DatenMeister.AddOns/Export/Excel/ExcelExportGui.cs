using BurnSystems.Test;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DatenMeister.AddOns.Export.Excel
{
    public static class ExcelExportGui
    {
        public static void AddMenu(IDatenMeisterWindow wnd, Func<IURIExtent> extentFactory)
        {
            Ensure.That(extentFactory != null);
            Ensure.That(wnd != null);

            var menuItem = new RibbonButton();
            menuItem.Label = Localization_DM_Addons.Menu_ExcelExport;
            menuItem.LargeImageSource = AddOnHelper.LoadIcon("x-office-spreadsheet.png");

            menuItem.Click += (x, y) =>
                {
                    var dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.Filter = Localization_DM_Addons.Filter_ExcelExport;
                    dlg.RestoreDirectory = true;
                    if (dlg.ShowDialog() == true)
                    {
                        // User has selected to store the excel file, now do
                        var excelExporter = new ExcelExport();
                        var excelSettings = new ExcelExportSettings();
                        excelSettings.Path = dlg.FileName;

                        excelExporter.ExportToFile(extentFactory(), excelSettings);

                        Process.Start(excelSettings.Path);
                    }
                };

            wnd.AddMenuEntry(Localization_DM_Addons.Menu_Export, menuItem);
        }
    }
}

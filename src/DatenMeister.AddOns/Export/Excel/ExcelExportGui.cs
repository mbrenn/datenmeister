﻿using BurnSystems.Test;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DatenMeister.AddOns.Export.Excel
{
    public static class ExcelExportGui
    {
        public static void AddMenu(IDatenMeisterWindow wnd, Func<IURIExtent> extentFactory)
        {
            Ensure.That(extentFactory != null);
            Ensure.That(wnd != null);

            var menuItem = new MenuItem();
            menuItem.Header = Localization_DM_Addons.Menu_ExcelExport;
            menuItem.Click += (x, y) =>
                {
                    var dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.Filter = Localization_DM_Addons.Filter_ExcelExport;
                    if (dlg.ShowDialog() == true)
                    {
                        // User has selected to store the excel file, now do
                        var excelExporter = new ExcelExport();
                        var excelSettings = new ExcelExportSettings();
                        excelSettings.Path = dlg.FileName;

                        excelExporter.ExportToFile(extentFactory(), excelSettings);
                    }
                };

            wnd.AddMenuEntry(Localization_DM_Addons.Menu_Extent, menuItem);
        }
    }
}
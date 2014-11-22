using BurnSystems.Test;
using DatenMeister.Logic;
using DatenMeister.Pool;
using DatenMeister.WPF.Modules.IconRepository;
using DatenMeister.WPF.Windows;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DatenMeister.AddOns.Export.Excel
{
    public static class ExcelExportGui
    {
        public static void AddMenu(IDatenMeisterWindow wnd)
        {
            Ensure.That(wnd != null);

            var menuItem = new RibbonButton();
            menuItem.Label = Localization_DM_Addons.Menu_ExcelExport;
            menuItem.LargeImageSource = Injection.Application.Get<IIconRepository>().GetIcon("spreadsheet");

            menuItem.Click += (x, y) =>
                {
                    try
                    {
                        var dataExtent = PoolResolver.GetDefaultPool().GetExtents(Logic.ExtentType.Data).First();

                        var dlg = new Microsoft.Win32.SaveFileDialog();
                        dlg.Filter = Localization_DM_Addons.Filter_ExcelExport;
                        dlg.RestoreDirectory = true;
                        if (dlg.ShowDialog() == true)
                        {
                            // User has selected to store the excel file, now do
                            var excelExporter = new ExcelExport();
                            var excelSettings = new ExcelExportSettings();
                            excelSettings.Path = dlg.FileName;

                            excelExporter.ExportToFile(dataExtent, excelSettings);

                            Process.Start(excelSettings.Path);
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("An Exception has occured: \r\n" + exc.Message);
                    }
                };

            wnd.AddMenuEntry(Localization_DM_Addons.Menu_Export, menuItem);
        }
    }
}

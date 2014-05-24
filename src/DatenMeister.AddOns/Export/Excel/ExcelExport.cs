using BurnSystems.Test;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.Export.Excel
{
    public class ExcelExport
    {
        public void ExportToFile(IURIExtent extent, ExcelExportSettings settings)
        {
            // Pop old culture and set to US, due to problems in NPOI
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            var oldUICulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            // Create the sheet
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("FirstSheet");

            var row = sheet.CreateRow(1);
            Ensure.That(row != null, "Row is null");
            var cell = row.CreateCell(5);

            Ensure.That(row != null, "Cell is null");
            cell.SetCellValue("Hallo, erster Inhalt!");

            using ( var fileStream = new FileStream ( settings.Path, FileMode.Create))
            {
                workbook.Write(fileStream);
            }

            // Restore the culture
            Thread.CurrentThread.CurrentCulture = oldCulture;
            Thread.CurrentThread.CurrentUICulture = oldUICulture;
        }
    }
}

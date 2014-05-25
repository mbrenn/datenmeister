using BurnSystems.Test;
using DatenMeister.Logic;
using NPOI.SS.UserModel;
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
        /// <summary>
        /// Stores the font for the header (might be a little bit more bold)
        /// </summary>
        private IFont headerfont;

        /// <summary>
        /// Stores the workbook for the current export
        /// </summary>
        private XSSFWorkbook workbook;

        public void ExportToFile(IURIExtent extent, ExcelExportSettings settings)
        {
            // Pop old culture and set to US, due to problems in NPOI
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            var oldUICulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            // Create the sheet
            this.workbook = new XSSFWorkbook();
            var sheet = this.workbook.CreateSheet("Export");

            // Creates the header font
            this.headerfont = this.workbook.CreateFont();
            this.headerfont.Boldweight = (short)FontBoldWeight.Bold;

            this.FillSheet(sheet, extent);

            using (var fileStream = new FileStream(settings.Path, FileMode.Create))
            {
                this.workbook.Write(fileStream);
            }

            // Restore the culture
            Thread.CurrentThread.CurrentCulture = oldCulture;
            Thread.CurrentThread.CurrentUICulture = oldUICulture;
        }

        /// <summary>
        /// Fills the sheet by the given extent. 
        /// </summary>
        /// <param name="sheet">Sheet to be used</param>
        /// <param name="extent">Extent being executed</param>
        private void FillSheet(NPOI.SS.UserModel.ISheet sheet, IURIExtent extent)
        {
            // Ok, first step... get all properties as a list
            var properties = extent.Elements().GetConsolidatedProperties();

            // Now set the header
            var row = sheet.CreateRow(0);
            var c = 0;
            foreach (var property in properties)
            {
                var cell = row.CreateCell(c);
                cell.SetCellValue(property);
                cell.CellStyle = this.workbook.CreateCellStyle();
                cell.CellStyle.SetFont(this.headerfont);
                cell.CellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                c++;
            }

            // Now include the rows
            var r = 1;
            foreach (var element in extent.Elements().Where(x => x is IObject).Select(x => x.AsIObject()))
            {
                row = sheet.CreateRow(r);
                c = 0;
                foreach (var property in properties)
                {
                    if (element.isSet(property))
                    {
                        var cell = row.CreateCell(c);
                        cell.SetCellValue(element.get(property).AsSingle().ToString());
                    }

                    c++;
                }

                r++;
            }

            // Now doing the autosize stuff
            for (var n = 0; n < properties.Count(); n++)
            {
                sheet.AutoSizeColumn(n);
            }
        }
    }
}

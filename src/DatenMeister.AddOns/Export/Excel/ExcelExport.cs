using BurnSystems.Test;
using DatenMeister.Logic;
using DatenMeister.Transformations.GroupBy;
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

            ///////////////////////////////////////
            // Prepare the start
            // Create the sheet
            this.workbook = new XSSFWorkbook();

            // Creates the header font
            this.headerfont = this.workbook.CreateFont();
            this.headerfont.Boldweight = (short)FontBoldWeight.Bold;
            
            ///////////////////////////////////////
            // Performs the fill
            if (settings.PerTypeOneSheet)
            {
                var inTypes = new GroupByTypeTransformation(extent.AsReflectiveCollection());
                foreach (var pairs in inTypes.ElementsAsGroupBy())
                {
                    var sheet = this.workbook.CreateSheet(pairs.key.AsIObject().get("name").AsSingle().ToString());
                    this.FillSheet(sheet, pairs.values);
                }
            }
            else
            {
                var sheet = this.workbook.CreateSheet("Export");
                this.FillSheet(sheet, extent.Elements());
            }

            ///////////////////////////////////////
            // Stores the changes
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
        /// <param name="elements">Extent being executed</param>
        private void FillSheet(NPOI.SS.UserModel.ISheet sheet, IReflectiveSequence elements)
        {
            // Ok, first step... get all properties as a list
            var properties = elements.GetConsolidatedPropertyNames();

            // Now set the header
            var row = sheet.CreateRow(0);

            // Creates the header for the type
            var cell = row.CreateCell(0);
            cell.SetCellValue("Type");
            cell.CellStyle = this.workbook.CreateCellStyle();
            cell.CellStyle.SetFont(this.headerfont);
            cell.CellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;

            // Creates the header for the values
            var c = 1;
            foreach (var property in properties)
            {
                cell = row.CreateCell(c);
                cell.SetCellValue(property);
                cell.CellStyle = this.workbook.CreateCellStyle();
                cell.CellStyle.SetFont(this.headerfont);
                cell.CellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                c++;
            }

            // Now include the rows
            var r = 1;
            foreach (var element in elements.Where(x => x is IObject).Select(x => x.AsIObject()))
            {
                row = sheet.CreateRow(r);

                // Sets the type of the element
                var typedElement = element as IElement;
                if (typedElement != null)
                {
                    cell = row.CreateCell(0);
                    var metaClass = typedElement.getMetaClass();
                    if (metaClass == null)
                    {
                        cell.SetCellValue(ObjectHelper.Null.ToString());
                    }
                    else
                    {
                        cell.SetCellValue(metaClass.get("name").AsSingle().ToString());
                    }
                }

                // Sets the values for each element
                c = 1;
                foreach (var property in properties)
                {
                    if (element.isSet(property))
                    {
                        cell = row.CreateCell(c);
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

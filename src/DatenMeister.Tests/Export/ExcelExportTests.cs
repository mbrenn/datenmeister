using DatenMeister.AddOns.Export.Excel;
using DatenMeister.Tests.DataProvider;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests.Export
{
    /// <summary>
    /// Defines the tests for the excel export
    /// </summary>
    [TestFixture]
    public class ExcelExportTests
    {
        /// <summary>
        /// Creates a file, but this file has to be opened by user, who tested it
        /// </summary>
        [Test]
        public void TestSimpleExport()
        {
            var extent = XmlTests.CreateTestExtent();

            var settings = new ExcelExportSettings();
            settings.Path = Path.Combine(
                Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),
                "test.xlsx");

            var excelExport = new ExcelExport();
            excelExport.ExportToFile(extent, settings);            
        }
    }
}

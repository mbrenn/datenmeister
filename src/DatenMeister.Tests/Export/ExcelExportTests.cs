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
        private static string testPath =
                Path.Combine(
                    Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),
                    "Depon.Net - Test Results");

        /// <summary>
        /// Gets the path for a certain fle
        /// </summary>
        /// <param name="filename">Name of the file</param>
        /// <returns>Found file</returns>
        private static string GetPathForFile(string filename)
        {
            if (!Directory.Exists(testPath))
            {
                Directory.CreateDirectory(testPath);
            }

            return Path.Combine(testPath, filename);
        }

        /// <summary>
        /// Creates a file, but this file has to be opened by user, who tested it
        /// </summary>
        [Test]
        public void TestSimpleExport()
        {
            var extent = XmlTests.CreateTestExtent();

            var settings = new ExcelExportSettings();
            settings.Path = GetPathForFile("Test 1 - Simple Xml Export.xlsx");

            var excelExport = new ExcelExport();
            excelExport.ExportToFile(extent, settings);
        }

        /// <summary>
        /// Creates a file, but this file has to be opened by user, who tested it
        /// </summary>
        [Test]
        public void TestByTypeExport()
        {
            var extent = XmlTests.CreateTestExtent();

            var settings = new ExcelExportSettings();
            settings.PerTypeOneSheet = true;
            settings.Path = GetPathForFile("Test 2 - Xml Export By Type.xlsx");

            var excelExport = new ExcelExport();
            excelExport.ExportToFile(extent, settings);
        }
    }
}

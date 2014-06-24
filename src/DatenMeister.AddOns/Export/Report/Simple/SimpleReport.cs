using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.Export.Report.Simple
{
    public class SimpleReport
    {
        /// <summary>
        /// Performs an export and stores the report in an html file
        /// </summary>
        /// <param name="collection">Collection of the elements to be stored into the report</param>
        /// <param name="path">Path of the html report</param>
        /// <param name="settings">The settings for the export into a report</param>
        public void Export(IReflectiveCollection collection, string path, SimpleReportSettings settings)
        {
            var text = "<body>BODY</body>";

            File.WriteAllText( path, text);

            try
            {
                Process.Start(path);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException(Localization_DM_Addons.Exception_ProcessStart_Html + exc.Message, exc);
            }
        }
    }
}

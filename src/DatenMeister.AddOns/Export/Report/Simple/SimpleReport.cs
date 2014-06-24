using DatenMeister.Transformations.GroupBy;
using DotLiquid;
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
            var template = Template.Parse(Localization_DM_Addons.SimpleReport_Template);
            
            // Create the tables
            var tables = new List<object>();
            var inTypes = new GroupByTypeTransformation(collection);
            foreach (var pair in inTypes.ElementsAsGroupBy())
            {
                var table = new { Headline = pair.key.AsSingle().AsIObject().get("name").AsSingle().ToString() };
                tables.Add(table);
            }
            var templateContent = template.Render(
                Hash.FromAnonymousObject(
                    new { 
                        Created = DateTime.Now.ToString(),
                        Tables = tables}));

            File.WriteAllText(path, templateContent);

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

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.CSV
{
    public class CSVDataProvider : IDataProvider
    {
        public CSVExtent Load(string source, CSVSettings settings)
        {
            var extent = new CSVExtent(source, settings);
            this.ReadFromFile(source, settings, extent);

            return extent;
        }

        /// <summary>
        /// Saves the extent into database.
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        /// <param name="path">Path, where file shall be stored</param>
        /// <param name="settings">Settings being used</param>
        public void Save(IURIExtent extent, string path, CSVSettings settings)
        {
            var columnHeaders = new List<string>();
            var extentAsCSV = extent as CSVExtent;

            // Retrieve the column headers
            if (settings.HasHeader && extentAsCSV != null && extentAsCSV.HeaderNames.Count > 0)
            {
                // Column headers given by old extent
                columnHeaders.AddRange(extentAsCSV.HeaderNames);
            }
            else
            {
                // Column headers given by number by asking each object about the number of properties and
                // then use the maximum value of the elements. This assumes that every element has the same type
                var maxColumnCount = extent.Elements().Select(x => x.AsIObject().getAll().Count()).Max();
                for (var n = 0; n < maxColumnCount; n++)
                {
                    columnHeaders.Add(string.Format("Column {0}", n));
                }
            }

            // Open File
            using (var streamWriter = new StreamWriter(path, false, settings.Encoding))
            {
                // Writes the header
                if (settings.HasHeader)
                {
                    this.WriteRow(streamWriter, settings, columnHeaders, x => x);
                }

                // Writes the elements
                foreach (var element in extent.Elements().Select(x => x.AsIObject()))
                {
                    this.WriteRow(
                        streamWriter,
                        settings,
                        columnHeaders,
                        x => element.get(x));
                }
            }
        }

        private void ReadFromFile(string path, CSVSettings settings, CSVExtent extent)
        {
            using (var stream = new StreamReader(path, settings.Encoding))
            {
                // Reads header, if necessary
                if (settings.HasHeader)
                {
                    extent.HeaderNames.AddRange(this.SplitLine(stream.ReadLine(), settings));
                }

                // Reads the data itself
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    var values = this.SplitLine(line, settings);

                    var csvObject = new CSVObject(extent, values);
                    extent.Objects.Add(csvObject);
                }
            }
        }

        /// <summary>
        /// Splits a CSV line into columns
        /// </summary>
        /// <returns>List of column values</returns>
        private IList<string> SplitLine(string line, CSVSettings settings)
        {
            return line.Split(new[] { settings.Separator }, StringSplitOptions.None);
        }

        /// <summary>
        /// Writes a columete
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="values"></param>
        private void WriteRow<T>(StreamWriter streamWriter, CSVSettings settings, IEnumerable<T> values, Func<T, object> conversion)
        {
            var builder = new StringBuilder();
            var first = true;
            foreach (var value in values)
            {
                if (!first)
                {
                    builder.Append(settings.Separator);
                }

                builder.Append(conversion(value).ToString());

                first = false;
            }

            streamWriter.WriteLine(builder.ToString());
        }
    }
}

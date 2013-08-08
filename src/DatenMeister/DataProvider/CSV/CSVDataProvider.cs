using System;
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
            throw new NotImplementedException();
        }

        public CSVExtent Import(string path, IURIExtent source, CSVSettings settings)
        {
            var extent = new CSVExtent(path, settings);

            return extent;
            throw new NotImplementedException();
        }

        public void StoreChanges()
        {
            throw new NotImplementedException();
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
    }
}

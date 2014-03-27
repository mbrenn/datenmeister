using BurnSystems.Collections;
using BurnSystems.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.CSV
{
    public class CSVObject : IObject
    {
        /// <summary>
        /// Defines the logger to be used
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(CSVObject));

        /// <summary>
        /// Stores the extent, which created the object
        /// </summary>
        private CSVExtent extent;

        /// <summary>
        /// Stores the value of the item
        /// </summary>
        private IList<string> values;

        /// <summary>
        /// Stores the line number of the CSV object
        /// </summary>
        private long line;

        public CSVObject(long line, CSVExtent extent, IList<string> values)
        {
            this.line = line;
            this.extent = extent;

            if (values != null)
            {
                this.values = values;
            }
            else
            {
                this.values = new List<string>();
            }
        }

        public object get(string propertyName)
        {
            lock (this.values)
            {
                int number = this.GetIndexOfProperty(propertyName);
                if (number == -1)
                {
                    throw new IndexOutOfRangeException();
                }

                return this.values[number];
            }
        }

        public IEnumerable<ObjectPropertyPair> getAll()
        {
            lock (this.values)
            {
                if (this.extent.Settings.HasHeader)
                {
                    foreach (var value in this.extent.HeaderNames)
                    {
                        yield return new ObjectPropertyPair(
                            value,
                            this.get(value));
                    }
                }
                else
                {
                    for (var n = 0; n < this.values.Count; n++)
                    {
                        yield return new ObjectPropertyPair(
                            "Column " + n.ToString(),
                            this.values[n]);
                    }
                }
            }
        }

        public bool isSet(string propertyName)
        {
            lock (this.values)
            {
                if (this.GetIndexOfProperty(propertyName) == -1)
                {
                    return false;
                }

                return true;
            }
        }

        public void set(string propertyName, object value)
        {
            lock (this.values)
            {
                var index = this.GetIndexOfProperty(propertyName);
                if (index != -1)
                {
                    this.values[index] = value.ToString();
                }
                else
                {
                    logger.Message("Could not set property name, due to unknown column: " + propertyName);
                }
            }
        }

        public bool unset(string propertyName)
        {
            lock (this.values)
            {
                var index = this.GetIndexOfProperty(propertyName);
                if (index != -1)
                {
                    this.values[index] = string.Empty;
                    return true;
                }

                return false;
            }
        }

        private int GetIndexOfProperty(string propertyName)
        {
            int number = -1;
            if (propertyName.StartsWith("Column "))
            {
                var numberString = propertyName.Substring("Column ".Length);
                if (Int32.TryParse(numberString, out number))
                {
                    if (number >= this.values.Count)
                    {
                        number = -1;
                    }
                }
            }

            if (number == -1)
            {
                number = extent.HeaderNames.IndexOf(propertyName);
            }

            // Fills the internal data structure to ensure that we can assign the value directly
            while (this.values.Count <= number && number > -1)
            {
                this.values.Add(string.Empty);
            }

            return number;
        }

        public void delete()
        {
            this.extent.RemoveObject(this);
        }

        /// <summary>
        /// Gets the id of the object
        /// </summary>
        public string Id
        {
            get { return this.line.ToString(); }
        }
    }
}

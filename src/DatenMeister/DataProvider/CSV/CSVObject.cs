using BurnSystems.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.CSV
{
    public class CSVObject : IObject
    {
        private CSVExtent extent;
        private IList<string> values;

        public CSVObject(CSVExtent extent, IList<string> values)
        {
            this.extent = extent;
            this.values = values;
        }

        public object Get(string propertyName)
        {
            int number = this.GetIndexOfProperty(propertyName);
            if (number == -1)
            {
                throw new IndexOutOfRangeException();
            }
            return this.values[number];
                
        }

        public IEnumerable<Pair<string, object>> GetAll()
        {
            if (this.extent.Settings.HasHeader)
            {
                foreach (var value in this.extent.HeaderNames)
                {
                    yield return new Pair<string, object>(
                        value, 
                        this.Get(value));
                }
            }
            else
            {
                for (var n = 0; n < this.values.Count; n++)
                {
                    yield return new Pair<string, object>(
                        "Column " + n.ToString(), 
                        this.values[n]);
                }
            }
        }

        public bool IsSet(string propertyName)
        {
            if (this.GetIndexOfProperty(propertyName) == -1)
            {
                return false;
            }

            return true;
        }

        public void Set(string propertyName, object value)
        {
            var index = this.GetIndexOfProperty(propertyName);
            if (index != -1)
            {
                this.values[index] = value.ToString();
            }
        }

        public void Unset(string propertyName)
        {
            var index = this.GetIndexOfProperty(propertyName);
            if (index != -1)
            {
                this.values[index] = string.Empty;
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

            return number;
        }
    }
}

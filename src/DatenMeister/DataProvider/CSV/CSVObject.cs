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
            if (propertyName.StartsWith("Column "))
            {
                var numberString = propertyName.Substring("Column ".Length);
                int number;
                if (Int32.TryParse(numberString, out number))
                {
                    if (number >= this.values.Count)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    return this.values[number];
                }
            }

            var index =  extent.HeaderNames.IndexOf ( propertyName );
            if (index == -1)
            {
                throw new IndexOutOfRangeException();
            }

            return this.values[index];
        }

        public KeyValuePair<string, object> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool IsSet(string propertyName)
        {
            if (propertyName.StartsWith("Column "))
            {
                var numberString = propertyName.Substring("Column ".Length);
                int number;
                if (Int32.TryParse(numberString, out number))
                {
                    if (number >= this.values.Count)
                    {
                        return false;
                    }

                    return true;
                }
            }

            var index = extent.HeaderNames.IndexOf(propertyName);
            if (index == -1)
            {
                return false;
            }

            return true;
        }

        public void Set(string propertyName, object value)
        {
            throw new NotImplementedException();
        }

        public void Unset(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}

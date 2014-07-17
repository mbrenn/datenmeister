using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    /// <summary>
    /// Defines a sequence of objects.
    /// TODO: Convert all the stuff
    /// </summary>
    public class DotNetSequence : IList<object>
    {
        private List<object> content = new List<object>();

        public IObject ConvertTo(object value)
        {
            if (value is DotNetObject)
            {
                return value as DotNetObject;
            }

            return new DotNetObject(null, value);
        }

        /// <summary>
        /// Initializes a new instance of the DotNetSequence clas
        /// </summary>
        public DotNetSequence()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DotNetSequence class and adds the array
        /// </summary>
        /// <param name="content">Objects to be added</param>
        public DotNetSequence(params object[] content)
        {
            this.content.AddRange(content.Select(x => ConvertTo(x)));
        }

        public int IndexOf(object item)
        {
            return this.content.IndexOf(ConvertTo(item));
        }

        public void Insert(int index, object item)
        {
            this.content.Insert(index, ConvertTo(item));
        }

        public void RemoveAt(int index)
        {
            this.content.RemoveAt(index);
        }

        public object this[int index]
        {
            get
            {
                return this.content[index];
            }
            set
            {
                this.content[index] = ConvertTo(value);
            }
        }

        public void Add(object item)
        {
            this.content.Add(ConvertTo(item));
        }

        public void Clear()
        {
            this.content.Clear();
        }

        public bool Contains(object item)
        {
            return this.content.Any(x => ConvertFrom(x).Equals(item));
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            this.content.CopyTo(array.Select(x=> ConvertTo(x)).ToArray(), arrayIndex);
        }

        public int Count
        {
            get { return this.content.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(object item)
        {
            return this.content.RemoveAll(x => ConvertFrom(x).Equals(item)) > 0;
        }

        public IEnumerator<object> GetEnumerator()
        {
            return this.content.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.content.GetEnumerator();
        }

        /// <summary>
        /// Converts the given item back to a dotnet object.
        /// If the element is a DotNet Object, the original value will be returned
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Returned object</returns>
        public object ConvertFrom(object value)
        {
            var valueAsDotNetObject = value as DotNetObject;
            if (valueAsDotNetObject != null)
            {
                return valueAsDotNetObject.Value;
            }

            return value;
        }
    }
}

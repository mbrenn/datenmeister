using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    /// <summary>
    /// Defines a sequence of objects. The extent is necessary to be able to perform all the necessary conversions
    /// </summary>
    public class DotNetSequence : IList<object>
    {
        /// <summary>
        /// Stores the dotnet extent for the type conversion
        /// </summary>
        private DotNetExtent dotNetExtent;

        private IList<object> content = new List<object>();

        /// <summary>
        /// Initializes a new instance of the DotNetSequence clas
        /// </summary>
        public DotNetSequence(DotNetExtent dotNetExtent)
        {
            this.dotNetExtent = dotNetExtent;
        }

        /// <summary>
        /// Initializes a new instance of the DotNetSequence class and adds the array
        /// </summary>
        /// <param name="content">Objects to be added</param>
        public DotNetSequence(DotNetExtent extent, params object[] content)
            : this(extent)
        {
            foreach (var item in content)
            {
                this.content.Add(item);
            }
        }

        /// <summary>
        /// Initializes a new instance of the DotNetSequence class and adds the array
        /// </summary>
        /// <param name="content">Objects to be added</param>
        public DotNetSequence(DotNetExtent extent, IList<object> content)
            : this(extent)
        {
            this.content = content;
        }

        public int IndexOf(object item)
        {
            return this.content.IndexOf(item);
        }

        public void Insert(int index, object item)
        {
            this.content.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.content.RemoveAt(index);
        }

        public object this[int index]
        {
            get
            {
                return this.ConvertTo(this.content[index]);
            }
            set
            {
                this.content[index] = value;
            }
        }

        public void Add(object item)
        {
            this.content.Add(item);
        }

        public void Clear()
        {
            this.content.Clear();
        }

        public bool Contains(object item)
        {
            return this.content.Any(x => x.Equals(item));
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            this.content.CopyTo(array.Select(x=> x).ToArray(), arrayIndex);
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
            return this.content.Remove(item);
        }

        public IEnumerator<object> GetEnumerator()
        {
            foreach ( var item in this.content)
            {
                yield return this.ConvertTo(item);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var item in this.content)
            {
                yield return this.ConvertTo(item);
            }
        }

        public DotNetObject ConvertTo(object value)
        {
            if (value is DotNetObject)
            {
                return value as DotNetObject;
            }

            var dotNetObject = new DotNetObject(null, value);
            dotNetObject.SetMetaClassByMapping(this.dotNetExtent);
            return dotNetObject;
        }
    }
}

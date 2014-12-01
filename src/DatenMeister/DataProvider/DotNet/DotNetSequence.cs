using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    /// <summary>
    /// Defines a sequence of objects. The extent is necessary to be able to perform all the necessary conversions
    /// </summary>
    public class DotNetSequence<T> : IList<object>
    {
        /// <summary>
        /// Stores the dotnet extent for the type conversion
        /// </summary>
        private DotNetExtent dotNetExtent;

        /// <summary>
        /// Gets or sets the flag, whether the sequence itself shall be regarded as read-only
        /// </summary>
        private bool isReadOnly = false;

        private IList<T> content = new List<T>();

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
        public DotNetSequence(DotNetExtent extent, params T[] content)
            : this(extent)
        {
            foreach (var item in content)
            {
                if (item is IEnumerable)
                {
                    throw new NotImplementedException("DotNetSequence currently does not support IEnumerables as content items");
                }

                this.content.Add(item);
            }
        }

        /// <summary>
        /// Initializes a new instance of the DotNetSequence class and adds the array
        /// </summary>
        /// <param name="content">Objects to be added</param>
        public static DotNetSequence<T> CreateFromList(DotNetExtent extent, IList<T> content)
        {
            var result = new DotNetSequence<T> ( extent);
            result.content = content;
            return result;
        }

        public int IndexOf(T item)
        {
            return this.content.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.RequireWrite();
            this.content.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.RequireWrite();
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
                this.RequireWrite();
                this.content[index] = (T)value;

                if ( this.content[index] == null && value != null)
                {
                    throw new InvalidOperationException("Value did not have the expected type. Has: " + value.GetType().ToString());
                }
            }
        }

        public void Add(T item)
        {
            this.RequireWrite();
            this.content.Add(item);
        }

        public void Clear()
        {
            this.RequireWrite();
            this.content.Clear();
        }

        public bool Contains(T item)
        {
            return this.content.Any(x => x.Equals(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.RequireWrite();
            this.content.CopyTo(array.Select(x=> x).ToArray(), arrayIndex);
        }

        public int Count
        {
            get { return this.content.Count; }
        }

        /// <summary>
        /// Gets or sets the value whether the sequence is read-only. 
        /// If the sequence is read-only, all requests for modification 
        /// will throw an exception. 
        /// </summary>
        public bool IsReadOnly
        {
            get { return this.isReadOnly; }
            set { this.isReadOnly = value; }
        }

        public bool Remove(T item)
        {
            this.RequireWrite();
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

        /// <summary>
        /// Converts the object to a dotnet object, if the value is a class object
        /// If this is just a native object, the object itself is returned without
        /// being converted to a DotNetObject
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Object, that is converted</returns>
        public object ConvertTo(object value)
        {
            if (value is DotNetObject)
            {
                return value as DotNetObject;
            }

            if (ObjectConversion.IsNative(value))
            {
                return value;
            }

            var dotNetObject = new DotNetObject(null, value);
            dotNetObject.SetMetaClassByMapping(this.dotNetExtent);
            return dotNetObject;
        }

        /// <summary>
        /// Requires write access, throws exception, when IsReadOnly flag is set. 
        /// </summary>
        private void RequireWrite()
        {
            if (this.isReadOnly)
            {
                throw new InvalidOperationException("DotNetSequence is read-only");
            }
        }

        int IList<object>.IndexOf(object item)
        {
            return this.IndexOf((T)item);
        }

        void IList<object>.Insert(int index, object item)
        {
            this.Insert(index, (T)item);            
        }

        void IList<object>.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        void ICollection<object>.Add(object item)
        {
            this.Add((T)item);
        }

        void ICollection<object>.Clear()
        {
            this.Clear();
        }

        bool ICollection<object>.Contains(object item)
        {
            return this.Contains ( (T)item );
        }

        void ICollection<object>.CopyTo(object[] array, int arrayIndex)
        {
            this.CopyTo(
                array.Select(x => (T)x).ToArray(),
                arrayIndex);
        }

        int ICollection<object>.Count
        {
            get { return this.Count; }
        }

        bool ICollection<object>.IsReadOnly
        {
            get { return this.IsReadOnly; }
        }

        bool ICollection<object>.Remove(object item)
        {
            return this.Remove((T)item);
        }
    }

    /// <summary>
    /// The non generic method
    /// </summary>
    public class DotNetSequence : DotNetSequence<object>
    {
        /// <summary>
        /// Initializes a new instance of the DotNetSequence clas
        /// </summary>
        public DotNetSequence(DotNetExtent dotNetExtent)
            : base(dotNetExtent)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DotNetSequence class and adds the array
        /// </summary>
        /// <param name="content">Objects to be added</param>
        public DotNetSequence(DotNetExtent extent, params object[] content)
            : base(extent, content)
        {
        }
    }
}

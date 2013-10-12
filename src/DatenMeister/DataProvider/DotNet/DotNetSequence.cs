using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    /// <summary>
    /// Defines a sequence of objects
    /// </summary>
    public class DotNetSequence : IList<object>
    {
        private List<object> content = new List<object>();

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
                return this.content[index];
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
            return this.content.Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            this.content.CopyTo(array, arrayIndex);
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
            return this.content.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.content.GetEnumerator();
        }
    }
}

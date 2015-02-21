using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.Common
{
    /// <summary>
    /// Implements a base reflective sequence, where all IEnumeration methods 
    /// are mapped to the IReflective Sequence methods
    /// </summary>
    public abstract class BaseReflectiveSequence : BaseReflectiveCollection, IReflectiveSequence
    {
        public BaseReflectiveSequence(IURIExtent extent)
            : base(extent)
        {
        }

        public abstract void add(int index, object value);

        public abstract object get(int index);

        public abstract object remove(int index);

        public abstract object set(int index, object value);

        int IList<object>.IndexOf(object item)
        {
            var n = 0;
            foreach ( var element in this)
            {
                if (element == item)
                {
                    return n;
                }

                n++;
            }

            return -1;
        }

        void IList<object>.Insert(int index, object item)
        {
            this.add(index, item);
        }

        void IList<object>.RemoveAt(int index)
        {
            this.remove(index);
        }

        object IList<object>.this[int index]
        {
            get
            {
                return this.get(index);
            }
            set
            {
                this.set(index, value);
            }
        }
    }
}

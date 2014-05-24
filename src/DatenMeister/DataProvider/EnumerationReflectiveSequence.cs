using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    public class EnumerationReflectiveSequence<T> : BaseReflectiveSequence
    {
        private IEnumerable<T> enumerable;

        public EnumerationReflectiveSequence(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
        }

        public override void add(int index, object value)
        {
            throw new NotImplementedException();
        }

        public override object get(int index)
        {
            return this.getAll().ElementAt(index);
        }

        public override object remove(int index)
        {
            throw new NotImplementedException();
        }

        public override object set(int index, object value)
        {
            throw new NotImplementedException();
        }

        public override bool add(object value)
        {
            throw new NotImplementedException();
        }

        public override void clear()
        {
            throw new NotImplementedException();
        }

        public override bool remove(object value)
        {
            throw new NotImplementedException();
        }

        public override int size()
        {
            return this.getAll().Count();
        }

        public override IEnumerable<object> getAll()
        {
            foreach (var item in this.enumerable)
            {
                yield return item;
            }
        }
    }
}

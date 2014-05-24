using DatenMeister.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Transformations
{
    /// <summary>
    /// This reflective sequence can be used as a proxy for the transformation. 
    /// Only getAll() needs to be overwritten. 
    /// </summary>
    public abstract class ProxyReflectiveSequence : BaseReflectiveSequence
    {
        protected IURIExtent baseExtent;

        public ProxyReflectiveSequence(IURIExtent extent)
        {
            this.baseExtent = extent;
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
            return this.baseExtent.Elements().remove(this.getAll().ElementAt(index));
        }

        public override object set(int index, object value)
        {
            throw new NotImplementedException();
        }

        public override bool add(object value)
        {
            return this.baseExtent.Elements().add(value);
        }

        public override void clear()
        {
            throw new NotImplementedException();
        }

        public override bool remove(object value)
        {
            return this.baseExtent.Elements().remove(value);
        }

        public override int size()
        {
            return this.getAll().Count();
        }

        public abstract override IEnumerable<object> getAll();
    }
}

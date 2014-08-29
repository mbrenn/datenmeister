using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Wrapper
{
    public class WrapperElement : IElement
    {
        /// <summary>
        /// Stores the inner element
        /// </summary>
        private IElement inner;

        internal IElement Inner
        {
            get { return this.inner; }
            set { this.inner = value; }
        }

        /// <summary>
        /// Gets or sets the wrapped extent
        /// </summary>
        internal IWrapperExtent WrapperExtent
        {
            get;
            set;
        }

        public WrapperElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the WrapperElement class
        /// </summary>
        /// <param name="element">Element to be added</param>
        public WrapperElement(IWrapperExtent extent, IElement element)
        {
            this.WrapperExtent = extent;
            this.inner = element;
        }

        public IElement Unwrap()
        {
            return this.inner;
        }

        public virtual IObject getMetaClass()
        {
            return this.inner.getMetaClass();
        }

        public virtual IObject container()
        {
            // TODO: Convert
            return this.WrapperExtent.Convert(this.inner.container()).AsIObject();
        }

        public virtual object get(string propertyName)
        {
            // TODO Convert
            return this.WrapperExtent.Convert(this.inner.get(propertyName));
        }

        public virtual IEnumerable<ObjectPropertyPair> getAll()
        {
            // TODO: Convert
            foreach (var pair in this.inner.getAll())
            {
                yield return new ObjectPropertyPair(pair.PropertyName, this.WrapperExtent.Convert(pair.Value));
            }
        }

        public virtual bool isSet(string propertyName)
        {
            return this.inner.isSet(propertyName);
        }

        public virtual void set(string propertyName, object value)
        {
            this.inner.set(propertyName, value);
        }

        public virtual bool unset(string propertyName)
        {
            return this.inner.unset(propertyName);
        }

        public virtual void delete()
        {
            this.inner.delete();
        }

        public virtual string Id
        {
            get { return this.inner.Id; }
        }

        public virtual IURIExtent Extent
        {
            get { return this.WrapperExtent; }
        }

        public override bool Equals(object obj)
        {
            if (obj is WrapperElement)
            {
                return Equals((obj as WrapperElement).Unwrap());
            }

            return this.Inner.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Inner.GetHashCode();
        }
    }
}

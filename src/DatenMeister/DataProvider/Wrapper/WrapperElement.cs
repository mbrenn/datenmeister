using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Wrapper
{
    public class WrapperElement : IElement, IProxyObject
    {
        /// <summary>
        /// Stores the inner element
        /// </summary>
        private IElement value;

        internal IElement Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Gets or sets the wrapped extent
        /// </summary>
        internal IWrapperExtent WrapperExtent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the full unwrapped element
        /// </summary>
        private IElement FullUnwrapped
        {
            get { return WrapperHelper.GetFullUnwrapped(this); }
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
            this.value = element;
        }

        public IElement Unwrap()
        {
            return this.value;
        }

        public virtual IObject getMetaClass()
        {
            return this.value.getMetaClass();
        }

        public virtual IObject container()
        {
            // TODO: Convert
            return this.WrapperExtent.Convert(this.value.container()).AsIObject();
        }

        public virtual object get(string propertyName)
        {
            // TODO Convert
            return this.WrapperExtent.Convert(this.value.get(propertyName));
        }

        public virtual IEnumerable<ObjectPropertyPair> getAll()
        {
            // TODO: Convert
            foreach (var pair in this.value.getAll())
            {
                yield return new ObjectPropertyPair(pair.PropertyName, this.WrapperExtent.Convert(pair.Value));
            }
        }

        public virtual bool isSet(string propertyName)
        {
            return this.value.isSet(propertyName);
        }

        public virtual void set(string propertyName, object value)
        {
            this.value.set(propertyName, value);
        }

        public virtual bool unset(string propertyName)
        {
            return this.value.unset(propertyName);
        }

        public virtual void delete()
        {
            this.value.delete();
        }

        public virtual string Id
        {
            get { return this.value.Id; }
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

            return this.Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Gets the value
        /// </summary>
        IObject IProxyObject.Value
        {
            get { return this.Value; }
        }

        /// <summary>
        /// Converts the element to a string element
        /// </summary>
        /// <returns>The value as a string representation</returns>
        public override string ToString()
        {
            return this.Value.ToString() + "*";
        }
    }
}

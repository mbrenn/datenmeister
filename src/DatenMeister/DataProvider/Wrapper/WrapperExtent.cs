using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Wrapper
{
    public interface IWrapperExtent : IURIExtent
    {
        object Convert(object value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TReflectiveSequence">Type of the wrapped reflective sequence</typeparam>
    /// <typeparam name="TElement">Type of the </typeparam>
    public class WrapperExtent<TReflectiveSequence, TElement, TUnspecified> : IWrapperExtent 
        where TReflectiveSequence : WrapperReflectiveSequence, new()
        where TElement : WrapperElement, new()
        where TUnspecified : WrapperUnspecified, new ()
    {
        /// <summary>
        /// Stores the inner extent
        /// </summary>
        private IURIExtent inner;

        protected IURIExtent Inner
        {
            get { return this.inner; }
            set { this.inner = value; }
        }

        protected WrapperExtent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the WrapperExtent class
        /// </summary>
        /// <param name="extent">Extent to be wrapped</param>
        public WrapperExtent(IURIExtent extent)
        {
            this.inner = extent;
        }

        /// <summary>
        /// Gets the wrapped original extent
        /// </summary>
        /// <returns></returns>
        public IURIExtent Unwrap()
        {
            return this.inner;
        }

        /// <summary>
        /// Wraps the extent
        /// </summary>
        /// <param name="extent">Extent to be wrapped</param>
        /// <returns>The wrapped instance</returns>
        public static WrapperExtent<TReflectiveSequence, TElement, TUnspecified> Wrap(IURIExtent extent)
        {
            return new WrapperExtent<TReflectiveSequence, TElement, TUnspecified>(extent);
        }

        public virtual string ContextURI()
        {
            return this.inner.ContextURI();
        }

        public virtual IReflectiveSequence Elements()
        {
            return this.CreateReflectiveSequence(this.inner.Elements());
        }

        public virtual IPool Pool
        {
            get
            {
                return this.inner.Pool;
            }
            set
            {
                this.inner.Pool = value;
            }
        }

        public virtual bool IsDirty
        {
            get
            {
                return this.inner.IsDirty;
            }
            set
            {
                this.inner.IsDirty = value;
            }
        }

        /// <summary>
        /// Creates an instance of the reflective instance
        /// </summary>
        /// <returns>Created reflective sequence</returns>
        public IReflectiveSequence CreateReflectiveSequence(IReflectiveSequence sequence)
        {
            var result = new TReflectiveSequence();
            result.WrapperExtent = this;
            result.Inner = sequence;
            return result;
        }

        /// <summary>
        /// Creates an instance of the reflective instance
        /// </summary>
        /// <returns>Created reflective sequence</returns>
        public IElement CreateElement(IElement element)
        {
            var result = new TElement();
            result.WrapperExtent = this;
            result.Inner = element;
            return result;
        }

        /// <summary>
        /// Creates an instance of the reflective instance
        /// </summary>
        /// <returns>Created reflective sequence</returns>
        public IUnspecified CreateUnspecified(IUnspecified element)
        {
            var result = new TUnspecified();
            result.WrapperExtent = this;
            result.Inner = element;
            return result;
        }

        /// <summary>
        /// Converts the object to wrapped instances if necessary. 
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Converted error</returns>
        public object Convert(object value)
        {
            if (Extensions.IsNative(value))
            {
                return value;
            }

            if (value is IElement)
            {
                return this.CreateElement(value as IElement);
            }

            if (value is IReflectiveSequence)
            {
                return this.CreateReflectiveSequence(value as IReflectiveSequence);
            }

            if (value is IUnspecified)
            {
                return this.CreateUnspecified(value as IUnspecified);
            }

            throw new NotImplementedException("Cannot conver type: " + value.ToString());
        }
    }
}

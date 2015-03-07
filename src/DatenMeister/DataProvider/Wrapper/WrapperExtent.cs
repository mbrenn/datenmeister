using DatenMeister.Logic;
using DatenMeister.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Wrapper
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TReflectiveSequence">Type of the wrapped reflective sequence</typeparam>
    /// <typeparam name="TElement">Type of the </typeparam>
    public class WrapperExtent<TReflectiveSequence, TElement> : IWrapperExtent 
        where TReflectiveSequence : WrapperReflectiveSequence, new()
        where TElement : WrapperElement, new()
    {
        /// <summary>
        /// Stores the inner extent
        /// </summary>
        private IURIExtent inner;

        /// <summary>
        /// Stores the fully unwrapped extents
        /// </summary>
        private IURIExtent fullUnwrappedExtent;

        protected IURIExtent Inner
        {
            get { return this.inner; }
            set { this.inner = value; }
        }

        /// <summary>
        /// Gets the full unwrapped extent as a cached element
        /// </summary>
        private IURIExtent FullUnwrappedExtent
        {
            get
            {
                if (this.fullUnwrappedExtent == null)
                {
                    this.fullUnwrappedExtent = WrapperHelper.GetFullUnwrapped(this.inner);
                }

                return this.fullUnwrappedExtent;
            }
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
        public static WrapperExtent<TReflectiveSequence, TElement> Wrap(IURIExtent extent)
        {
            return new WrapperExtent<TReflectiveSequence, TElement>(extent);
        }

        public virtual string ContextURI()
        {
            return this.inner.ContextURI();
        }

        public virtual IReflectiveSequence Elements()
        {
            return this.CreateReflectiveSequence(this.inner.Elements());
        }

        /// <summary>
        /// Gets or sets the pool
        /// </summary>
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

        /// <summary>
        /// Gets or sets the flag indicating whether the extent is dirty
        /// </summary>
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
            result.Value = element;
            return result;
        }

        /// <summary>
        /// Converts the object to wrapped instances if necessary. 
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Converted error</returns>
        public object Convert(object value)
        {
            if (ObjectConversion.IsNative(value))
            {
                return value;
            }

            value = value.FullResolve();

            if (value == ObjectHelper.NotSet || value == ObjectHelper.Null)
            {
                return value;
            }

            if (value is IElement)
            {
                var valueAsElement = value as IElement;
                var fullUnwrappedExtent = WrapperHelper.GetFullUnwrapped(valueAsElement.Extent);

                if (this.FullUnwrappedExtent == fullUnwrappedExtent)
                {
                    return this.CreateElement(valueAsElement);
                }
                else
                {
                    return valueAsElement;
                }
            }

            if (value is IReflectiveSequence)
            {
                var valueAsReflectiveSequence = value as IReflectiveSequence;

                var fullUnwrappedExtent = WrapperHelper.GetFullUnwrapped(valueAsReflectiveSequence.Extent);
                if (this.FullUnwrappedExtent == fullUnwrappedExtent)
                {
                    return this.CreateReflectiveSequence(valueAsReflectiveSequence);
                }
                else
                {
                    return valueAsReflectiveSequence;
                }
            }

            throw new NotImplementedException("Cannot convert type: " + value.ToString());
        }

        public override bool Equals(object obj)
        {
            if (obj is IWrapperExtent)
            {
                return Equals((obj as IWrapperExtent).Unwrap());
            }

            return this.Inner.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Inner.GetHashCode();
        }
    }
}

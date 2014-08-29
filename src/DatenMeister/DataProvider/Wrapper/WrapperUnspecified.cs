using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Wrapper
{
    /// <summary>
    /// Wraps the IUnspecified class
    /// </summary>
    public class WrapperUnspecified : IUnspecified
    {
        /// <summary>
        /// Stores the inner element
        /// </summary>
        private IUnspecified inner;

        internal IUnspecified Inner
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

        public WrapperUnspecified()
        {
        }

        /// <summary>
        /// Initializes a new instance of the WrapperElement class
        /// </summary>
        /// <param name="inner">Element to be added</param>
        public WrapperUnspecified(IWrapperExtent extent, IUnspecified inner)
        {
            this.WrapperExtent = extent;
            this.inner = inner;
        }

        public IUnspecified Unwrap()
        {
            return this.inner;
        }

        public PropertyValueType PropertyValueType
        {
            get
            {
                return this.Inner.PropertyValueType;
            }
            set
            {
                this.Inner.PropertyValueType = value;
            }
        }

        public object AsSingle()
        {
            return this.WrapperExtent.Convert(this.Inner.AsSingle());
        }

        public IReflectiveCollection AsReflectiveCollection()
        {
            var result = this.WrapperExtent.Convert(this.Inner.AsReflectiveCollection()) as IReflectiveCollection;
            Ensure.That(result != null, "Wrapper Extent did not return a reflective collection");
            return result;
        }

        public IReflectiveSequence AsReflectiveSequence()
        {
            var result = this.WrapperExtent.Convert(this.Inner.AsReflectiveSequence()) as IReflectiveSequence;
            Ensure.That(result != null, "Wrapper Extent did not return a reflective sequence");
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj is WrapperUnspecified)
            {
                return Equals((obj as WrapperUnspecified).Unwrap());
            }

            return this.Inner.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Inner.GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Wrapper
{
    /// <summary>
    /// Contains some helper methods
    /// </summary>
    public static class WrapperHelper
    {
        /// <summary>
        /// Checks, if the given item matches the type X. 
        /// If not, unwrap the extent if possible and redo the check
        /// </summary>
        /// <typeparam name="X">Extent, which needs to be found</typeparam>
        /// <param name="extent">Extent to be unwrapped</param>
        /// <returns>The found extent or null, if not found</returns>
        public static X FindWrappedExtent<X>(IURIExtent extent) where X : class, IWrapperExtent
        {
            while (extent != null)
            {
                var found = extent as X;
                if (found != null)
                {
                    return found;
                }

                var wrapped = extent as IWrapperExtent;
                if (wrapped != null)
                {
                    extent = wrapped.Unwrap();
                }
                else
                {
                    // Nothing found, we are out
                    extent = null;
                }
            }

            return null;
        }

        /// <summary>
        /// Performs a total unwrap of the given extent
        /// </summary>
        /// <param name="extent">Extent to be unwrapped</param>
        /// <returns>The unwrapped extent</returns>
        public static IURIExtent GetFullUnwrapped(IURIExtent extent)
        {
            while (extent is IWrapperExtent)
            {
                extent = (extent as IWrapperExtent).Unwrap();
            }

            return extent;
        }

        /// <summary>
        /// Performs a total unwrap of the given extent
        /// </summary>
        /// <param name="element">Extent to be unwrapped</param>
        /// <returns>The unwrapped extent</returns>
        public static IElement GetFullUnwrapped(IElement element)
        {
            while (element is WrapperElement)
            {
                element = (element as WrapperElement).Unwrap();
            }

            return element;
        }

        /// <summary>
        /// Performs a total unwrap of the given extent
        /// </summary>
        /// <param name="sequence">Extent to be unwrapped</param>
        /// <returns>The unwrapped extent</returns>
        public static IReflectiveSequence GetFullUnwrapped(IReflectiveSequence sequence)
        {
            while (sequence is WrapperReflectiveSequence)
            {
                sequence = (sequence as WrapperReflectiveSequence).Unwrap();
            }

            return sequence;
        }

        /// <summary>
        /// Performs a total unwrap of the given extent
        /// </summary>
        /// <param name="unspecified">Extent to be unwrapped</param>
        /// <returns>The unwrapped extent</returns>
        public static IUnspecified GetFullUnwrapped(IUnspecified unspecified)
        {
            while (unspecified is WrapperUnspecified)
            {
                unspecified = (unspecified as WrapperUnspecified).Unwrap();
            }

            return unspecified;
        }
    }
}

using BurnSystems.Test;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.DataProvider.Wrapper;
using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// This factory provider uses the type of the given extent to create a correct
    /// factory which is suitable for the extent. 
    /// </summary>
    public class FactoryProvider : IFactoryProvider
    {
        /// <summary>
        /// Stores a list of customer factories, that can be defined
        /// certain specific factories. 
        /// </summary>
        private Dictionary<Type, Func<Type, IURIExtent, IFactory>> customFactories =
            new Dictionary<Type, Func<Type, IURIExtent, IFactory>>();

        /// <summary>
        /// Creates a factory for the given extent
        /// </summary>
        /// <param name="extent">Extent which is used to create the factory</param>
        /// <returns>The created factory</returns>
        public IFactory CreateFor(Type type, IURIExtent extent)
        {
            Ensure.That(type != null, "Type == null");

            Func<Type, IURIExtent, IFactory> method;
            if (this.customFactories.TryGetValue(type, out method))
            {
                return method(type, extent);
            }

            // Nothing find, go to general table
            return GetDefaultFactory(type, extent);
        }

        /// <summary>
        /// Adds a customer factory to the given factory extent. 
        /// </summary>
        /// <param name="type">Type of the extent, for which the objects will be created</param>
        /// <param name="customFactoryMethod">Factory method being used to create the factory</param>
        public void AddCustomFactory(Type type, Func<Type, IURIExtent, IFactory> customFactoryMethod)
        {
            Ensure.That(type != null);
            Ensure.That(customFactoryMethod != null); 

            this.customFactories[type] = customFactoryMethod;
        }
         
        /// <summary>
        /// Gets the default factory by type an extent
        /// </summary>
        /// <param name="type">Type to be querie</param>
        /// <param name="extent">Extent being used to create the objects</param>
        /// <returns>Created factory</returns>
        private static IFactory GetDefaultFactory(Type type, IURIExtent extent)
        {
            if (type == typeof(Generic.GenericExtent))
            {
                return new Generic.GenericFactory(extent as Generic.GenericExtent);
            }

            if (type == typeof(Xml.XmlExtent))
            {
                return new Xml.XmlFactory(extent as Xml.XmlExtent);
            }

            if (type == typeof(DotNet.DotNetExtent))
            {
                return new DatenMeister.DataProvider.DotNet.DotNetFactory(extent as DotNet.DotNetExtent);
            }

            if (type == typeof(DotNet.GlobalDotNetExtent))
            {
                return new DatenMeister.DataProvider.DotNet.DotNetFactory(extent as DotNet.DotNetExtent);
            }

            if (type == typeof(CSV.CSVExtent))
            {
                return new DatenMeister.DataProvider.CSV.CSVFactory(extent as CSV.CSVExtent);
            }

            if (type.GetInterfaces().Any(x => x == typeof(IWrapperExtent)))
            {
                var unwrapped = (extent as IWrapperExtent).Unwrap();
                return GetDefaultFactory(unwrapped.GetType(), unwrapped);
            }

            throw new NotImplementedException("The type of the given IURIExtent is unknown: " + extent.GetType().ToString());
        }
    }
}

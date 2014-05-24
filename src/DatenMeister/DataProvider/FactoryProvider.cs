using BurnSystems.Test;
using DatenMeister.DataProvider.DotNet;
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
        /// Creates a factory for the given extent
        /// </summary>
        /// <param name="extent">Extent which is used to create the factory</param>
        /// <returns>The created factory</returns>
        public IFactory CreateFor(IURIExtent extent)
        {
            Ensure.That(extent != null, "Extent == null");
            if (extent is Xml.XmlExtent)
            {
                return new Xml.XmlFactory(extent);
            }

            if (extent is DotNet.DotNetExtent)
            {
                throw new NotImplementedException("DotNet Type not implemented");
            }

            if (extent is CSV.CSVExtent)
            {
                return new DatenMeister.DataProvider.CSV.CSVFactory(extent as CSV.CSVExtent);
            }

            throw new NotImplementedException("The type of the given IURIExtent is unknown: " + extent.GetType().ToString());
        }
    }
}

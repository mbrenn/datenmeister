﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// A factory provider uses the type of the given extent to create a correct
    /// factory which is suitable for the extent. 
    /// </summary>
    public interface IFactoryProvider
    {
        /// <summary>
        /// Creates a factory for the given extent
        /// </summary>
        /// <param name="type">Type to be used</param>
        /// <param name="extent">Extent which is used to create the factory</param>
        /// <returns>The created factory</returns>
        IFactory CreateFor(Type type, IURIExtent extent);
    }
}

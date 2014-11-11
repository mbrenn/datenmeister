using DatenMeister.Logic;
using DatenMeister.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.Data.FileSystem
{
    /// <summary>
    /// Initializes all necessary instances and classes for the filesystem
    /// </summary>
    public class Init
    {
        /// <summary>
        /// Performs the initialization and adds all necessary items into the pool
        /// </summary>
        /// <param name="pool">Pool to be used</param>
        public void Do(IPool pool)
        {
            var typeExtent = pool.GetExtent(ExtentType.Type);
            

        }
    }
}

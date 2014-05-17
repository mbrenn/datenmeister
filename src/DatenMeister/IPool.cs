using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister
{
    public interface IPool
    {
        /// <summary>
        /// Gets a list of extent instances. 
        /// This method shall be thread-safe
        /// </summary>
        IEnumerable<ExtentInstance> Instances
        {
            get;
        }
    }
}

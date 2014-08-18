using DatenMeister.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.ComplianceSuite
{
    /// <summary>
    /// Runs the compliance suite for a given extent and type extent 
    /// </summary>
    public class Suite
    {
        /// <summary>
        /// Stores the extent factory
        /// </summary>
        private Func<IURIExtent> extentFactory
        {
            get;
            set;
        }

        private Func<IObject> objectFactory
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the suite for a given extent, where the data shall be stored.
        /// </summary>
        /// <param name="extent">Factory for an empty extent, which shall be checked for compliance</param>
        public Suite(Func<IURIExtent> extentFactory, Func<IObject> objectFactory)
        {
            this.extentFactory = extentFactory;
            this.objectFactory = objectFactory;
        }

        /// <summary>
        /// Runs the compliance suite
        /// </summary>
        /// <returns>An object, containing the information of all the passed tests</returns>
        public IObject Run()
        {
            var result = new GenericObject();

            var mofObjectCompliance = new MofObjectCompliance(this.objectFactory, result);
            mofObjectCompliance.Run();

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.ComplianceSuite
{
    public class MofCompliance
    {
        /// <summary>
        /// Stores the result of the storage
        /// </summary>
        private IObject resultStorage;

        /// <summary>
        /// Empty test object, where getting and setting will be 
        /// executed and tested.
        /// </summary>
        private IObject testObject;

        public MofCompliance(IObject testObject, IObject resultStorage)
        {
            this.testObject = testObject;
            this.resultStorage = resultStorage;
        }

        public void ExecuteTests()
        {
        }
    }
}

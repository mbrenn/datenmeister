using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.ComplianceSuite
{
    /// <summary>
    /// Checks the compliance to chapter 9.3 of the MOF CoreSpecification 2.4.1
    /// </summary>
    public class MofObjectCompliance
    {
        /// <summary>
        /// Stores the result of the storage
        /// </summary>
        private IObject resultStorage;

        /// <summary>
        /// Empty test object, where getting and setting will be 
        /// executed and tested.
        /// </summary>
        private Func<IObject> testObjectFactory;

        /// <summary>
        /// Initialiezs a new instance of the MOF Tests
        /// </summary>
        /// <param name="testObjectFactory"></param>
        /// <param name="resultStorage"></param>
        public MofObjectCompliance(Func<IObject> testObjectFactory, IObject resultStorage)
        {
            this.testObjectFactory = testObjectFactory;
            this.resultStorage = resultStorage;
        }

        public void Run()
        {
            this.ExecuteOperationsEqualsTest();
        }

        /// <summary>
        /// Checks chapter 9.3.1 Operations...
        /// Incomplete test, since no difference between Instance Types and Data Types yet implemented
        /// </summary>
        private void ExecuteOperationsEqualsTest()
        {
            var correct = true;
            try
            {
                var instance1 = this.testObjectFactory();
                var instance2 = this.testObjectFactory();

                if (!instance1.Equals(instance1))
                {
                    correct &= false;
                }

                if (instance1.Equals(instance2))
                {
                    correct &= false;
                }

                if (instance2.Equals(instance1))
                {
                    correct &= false;
                }
            }
            catch
            {
                correct = false;
            }

            this.resultStorage.set("Compliance.MOF.9.3.1.equals.instances", correct);
            this.resultStorage.set("Compliance.MOF.9.3.1.equals.datatype", null); // Not tested
        }
    }
}

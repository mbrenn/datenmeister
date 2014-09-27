using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.ComplianceSuite
{
    public class BaseSuite
    {
        /// <summary>
        /// Stores the result of the storage
        /// </summary>
        private IObject ResultStorage
        {
            get;
            set;
        }

        public BaseSuite(IObject resultStorage)
        {
            this.ResultStorage = resultStorage;
        }

        /// <summary>
        /// Executes the test vector. 
        /// If the test vector return true, the name of the test will be set as true. 
        /// If it returns false or throws an exception, the test vector will be set as false
        /// </summary>
        /// <param name="name"></param>
        /// <param name="testVector"></param>
        protected void Test(
            string name,
            Func<bool> testVector)
        {
            try
            {
                if (testVector())
                {
                    this.ResultStorage.set(name, true);
                }
                else
                {
                    this.ResultStorage.set(name, false);
                }
            }
            catch(Exception exc)
            {
                this.ResultStorage.set(name,
                    string.Format(
                        "{0} - {1}",
                        exc.GetType().Name,
                        exc.Message));
            }
        }

        /// <summary>
        /// Indicates that the test is not run
        /// </summary>
        /// <param name="name">Name of the test being not run</param>
        protected void NoTest(string name)
        {
            this.ResultStorage.set(name, "Not Run");
        }
    }
}

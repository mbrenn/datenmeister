using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.ComplianceSuite.Mof
{
    /// <summary>
    /// Checks the compliance to chapter 9.3 of the MOF CoreSpecification 2.4.1
    /// </summary>
    public class Chapter9Tests : BaseSuite
    {

        /// <summary>
        /// Empty test object, where getting and setting will be 
        /// executed and tested.
        /// </summary>
        private Tests suite;

        /// <summary>
        /// Initialiezs a new instance of the MOF Tests
        /// </summary>
        /// <param name="testObjectFactory"></param>
        /// <param name="resultStorage"></param>
        public Chapter9Tests(Tests suite, IObject resultStorage)
            : base(resultStorage)
        {
            this.suite = suite;
        }

        public void Run()
        {
            this.Test_Chapter_9_3_1_equals();
            this.Test_Chapter_9_3_1_get();
            this.Test_Chapter_9_3_1_isSet();
            this.Test_Chapter_9_3_1_unset();
        }

        /// <summary>
        /// Checks chapter 9.3.1 Operations...
        /// Incomplete test, since no difference between Instance Types and Data Types yet implemented
        /// </summary>
        private void Test_Chapter_9_3_1_equals()
        {
            var correct = true;
            this.Test("Compliance.MOF.9.3.1.equals.instances",
                () =>
                {
                    var extent = this.suite.ExtentFactory();
                    var instance1 = this.suite.ObjectFactory(extent);
                    var instance2 = this.suite.ObjectFactory(extent);

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

                    return correct;
                });

            this.NoTest("Compliance.MOF.9.3.1.equals.datatype"); // Not tested
        }

        private void Test_Chapter_9_3_1_get()
        {
            var extent = this.suite.ExtentFactory();
            var instance1 = this.suite.ObjectFactory(extent);
            // Test getting unknown properties
            this.Test("Compliance.MOF.9.3.1.get.unknown",
                () => instance1.getAsSingle("unknown") == ObjectHelper.NotSet);

            // Test getting integers after having them set
            this.Test("Compliance.MOF.9.3.1.get.integer",
                () =>
                {
                    var success = true;
                    instance1.set("hasValue", 2);
                    var result = instance1.getAsSingle("hasValue");
                    success = success ?
                        (ObjectConversion.ToInt32(result) == 2) :
                        false;
                    return success;
                });


            // Test getting string after having them set
            this.Test("Compliance.MOF.9.3.1.get.string",
                () =>
                {
                    var success = true;
                    instance1.set("hasAnotherValue", "value");
                    var result = instance1.getAsSingle("hasAnotherValue");
                    success &= result is string;
                    success = success ?
                        (ObjectConversion.ToString(result) == "value") :
                        false;
                    return success;
                });

            // Test getting a reflective sequence
            this.Test("Compliance.MOF.9.3.1.get.asReflectiveSequence",
                () =>
                {
                    // Test getting reflective sequence after having them set
                    var success = true;
                    var result = instance1.getAsReflectiveSequence("mySequence");
                    success = success ?
                        result is IReflectiveSequence :
                        false;
                    return success;
                });

            // Test adding innerObject
            this.Test("Compliance.MOF.9.3.1.get.innerObject",
                () =>
                {
                    var success = true;
                    var innerObject = this.suite.ObjectFactory(extent);

                    instance1.set("innerObject", innerObject);
                    var result = instance1.get("innerObject").AsIObject();
                    success = success ?
                        result.Id.Equals(innerObject.Id) :
                        false;
                    return success;
                });
        }

        private void Test_Chapter_9_3_1_isSet()
        {
            var extent = this.suite.ExtentFactory();
            var instance1 = this.suite.ObjectFactory(extent);

            this.Test("Compliance.MOF.9.3.1.isSet.unknown",
                () => instance1.isSet("unknown") == false);

            this.Test("Compliance.MOF.9.3.1.isSet.known",
                () =>
                {
                    instance1.set("known", true);
                    return instance1.isSet("known") == true;
                });
        }

        private void Test_Chapter_9_3_1_unset()
        {
            var extent = this.suite.ExtentFactory();
            var instance1 = this.suite.ObjectFactory(extent);

            this.Test("Compliance.MOF.9.3.1.unset",
                () =>
                {
                    var success = true;
                    success = success ? instance1.isSet("known") == false : false;
                    success = success ? ObjectConversion.IsNull(instance1.get("known")) : false;
                    instance1.set("known", true);
                    success = success ? instance1.isSet("known") == true : false;
                    success = success ? (!ObjectConversion.IsNull(instance1.get("known"))) : false;
                    instance1.unset("known");
                    success = success ? instance1.isSet("known") == false : false;
                    success = success ? ObjectConversion.IsNull(instance1.get("known")) : false;
                    instance1.unset("known");
                    success = success ? instance1.isSet("known") == false : false;
                    success = success ? ObjectConversion.IsNull(instance1.get("known")) : false;
                    return success;
                });
        }
    }
}

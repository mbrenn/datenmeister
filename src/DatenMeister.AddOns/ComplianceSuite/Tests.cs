using DatenMeister.AddOns.ComplianceSuite.Mof;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.AddOns.ComplianceSuite
{
    /// <summary>
    /// Runs the compliance suite for a given extent and type extent 
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// Stores the extent factory
        /// </summary>
        public Func<IURIExtent> ExtentFactory
        {
            get;
            set;
        }

        /// <summary>
        /// Creates another object. The object shall not be added to the 
        /// extent
        /// </summary>
        public Func<IURIExtent, IObject> ObjectFactory
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the suite for a given extent, where the data shall be stored.
        /// </summary>
        /// <param name="extent">Factory for an empty extent, which shall be checked for compliance</param>
        public Tests(Func<IURIExtent> extentFactory, Func<IURIExtent, IObject> objectFactory)
        {
            this.ExtentFactory = extentFactory;
            this.ObjectFactory = objectFactory;
        }

        /// <summary>
        /// Runs the compliance suite
        /// </summary>
        /// <returns>An object, containing the information of all the passed tests</returns>
        public IObject Run()
        {
            var result = new GenericObject();

            var mofObjectCompliance = new Chapter9Tests(this, result);
            mofObjectCompliance.Run();

            return result;
        }

        /// <summary>
        /// Gets a suite for generic objects
        /// </summary>
        public static Tests Generic
        {
            get
            {
                // Test for Generic Extent
                Func<IURIExtent> factoryGeneric = () => new GenericExtent("datenmeister:///temp");
                Func<IURIExtent, IObject> factoryObject = (x) => new GenericObject(x);
                return new ComplianceSuite.Tests(factoryGeneric, factoryObject);                
            }
        }

        /// <summary>
        /// Gets a suite for generic objects
        /// </summary>
        public static Tests Xml
        {
            get
            {
                // Test for Generic Extent
                Func<IURIExtent> factoryGeneric = () => new XmlExtent(
                    new System.Xml.Linq.XDocument(
                        new XElement("elements")),
                    "datenmeister:///temp");

                Func<IURIExtent, IObject> factoryObject = (x) =>
                {
                    var factory = new XmlFactory(x as XmlExtent);
                    return factory.create(null);
                };

                return new ComplianceSuite.Tests(factoryGeneric, factoryObject);
            }
        }
    }
}

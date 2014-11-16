using BurnSystems.Logging;
using DatenMeister.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.Entities.AsObject.Uml
{
    public static partial class Types
    {
        /// <summary>
        /// Stores the logger for the extent
        /// </summary>
        public static ILog logger = new ClassLogger(typeof(Types));

        /// <summary>
        /// Stores the xmi namespace being used to define the types
        /// </summary>
        public static readonly XNamespace XmiNamespace = "http://www.omg.org/spec/XMI/2.4.1";

        /// <summary>
        /// Stores the namespace for the Xml
        /// </summary>
        public static readonly XNamespace XmlNamespace = "http://www.w3.org/2000/xmlns/";

        /// <summary>
        /// Resets the type for the internal object
        /// </summary>
        static partial void OnInitCompleted()
        {
            if (Types.Type is GenericElement)
            {
                // We can only do correct initialization of UML, when we can change the metaclass
                (Types.Type as GenericElement).setMetaClass(Types.Class);
                (Types.NamedElement as GenericElement).setMetaClass(Types.Class);
                (Types.Property as GenericElement).setMetaClass(Types.Class);
                (Types.Class as GenericElement).setMetaClass(Types.Class);
            }
            else
            {
                logger.Message("The given extent is not a GenericExtent, so the UML types do not have the correct meta class");
            }
        }

        /// <summary>
        /// Performs the initialization in a way that all
        /// the types are stored in a GenericExtent. By defeult, the initialization
        /// is done in a DotNetExtent, which is not possible for the UML stuff
        /// </summary>
        public static IURIExtent InitDecoupled()
        {
            var genericExtent = new GenericExtent("datenmeister:///metatypes/");
            Init(genericExtent);
            return genericExtent;
        }
    }

}

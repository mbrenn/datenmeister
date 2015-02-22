using DatenMeister.DataProvider.Generic;
using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.DataProvider.Xml
{
    public static class XmlRepresentation
    {
        /// <summary>
        /// Creates an xml document out of the reflective collection and returns it.
        /// This method can be used for debugging and other purposes
        /// </summary>
        /// <param name="collection">Collection to be converted</param>
        /// <param name="extentUri">The extent uri to be used for export</param>
        /// <returns>The document being created</returns>
        public static XDocument GetByCollection(IReflectiveCollection collection)
        {
            var extentUri = "datenmeister:///temp/" + Guid.NewGuid().ToString();

            // Prepare extent, receiving the copy
            var copiedExtent = new XmlExtent(
                XDocument.Parse("<export />"),
                extentUri);
            copiedExtent.XmlDocument.AddAnnotation(SaveOptions.OmitDuplicateNamespaces);

            // Executes the copying
            var copier = new ExtentCopier(collection, copiedExtent.Elements());
            copier.Copy();

            return copiedExtent.XmlDocument;
        }

        /// <summary>
        /// Created
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static XDocument GetByEnumeration(IEnumerable<object> values)
        {
            var extentUri = "datenmeister:///temp/" + Guid.NewGuid().ToString();

            var genericExtent = new GenericExtent(extentUri);
            foreach (var value in values)
            {
                genericExtent.Elements().add(value);
            }

            return GetByCollection(genericExtent.Elements());
        }
    }
}

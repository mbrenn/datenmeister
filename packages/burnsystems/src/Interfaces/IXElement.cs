//-----------------------------------------------------------------------
// <copyright file="IXElement.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Interfaces
{
    using System.Xml.Linq;

    /// <summary>
    /// This interface is implemented by all classes, whose
    /// state shall be read from an xml node or shall be written to 
    /// an xml node
    /// </summary>
    public interface IXElement
    {
        /// <summary>
        /// Reads properties and attributes from an xml node
        /// </summary>
        /// <param name="xmlNode">Xmlnode, which should be read.</param>
        void ReadFromXmlNode(XElement xmlNode);

        /// <summary>
        /// Stores the properties of the object into the xml node
        /// </summary>
        /// <param name="xmlNode">Xmlnode, where properties are stored</param>
        void WriteIntoXmlNode(XElement xmlNode);
    }
}

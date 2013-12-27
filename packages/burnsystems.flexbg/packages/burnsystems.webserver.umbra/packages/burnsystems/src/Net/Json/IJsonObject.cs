//-----------------------------------------------------------------------
// <copyright file="IJsonObject.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Net.Json
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This interface has to be implemented by all objects that can
    /// be used as json object. 
    /// The interface has no properties or methods, it is only used for 
    /// declaration of objects for conversion.
    /// </summary>
    [Obsolete("Use System.Web.Script.Serialization from System.Web.Extensions.dll")]
    public interface IJsonObject
    {
    }
}

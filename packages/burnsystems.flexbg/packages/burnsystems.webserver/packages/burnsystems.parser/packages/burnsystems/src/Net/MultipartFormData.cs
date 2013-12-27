//-----------------------------------------------------------------------
// <copyright file="MultipartFormData.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Net
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements the storage of a multipart formdata according
    /// to RFC 2388
    /// </summary>
    public class MultipartFormData
    {
        /// <summary>
        /// List of parts
        /// </summary>
        private List<MultipartFormDataPart> parts =
            new List<MultipartFormDataPart>();

        /// <summary>
        /// Gets the different parts of the formdata
        /// </summary>
        public List<MultipartFormDataPart> Parts
        {
            get { return this.parts; }
        }
    }
}

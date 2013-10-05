using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Web
{
    /// <summary>
    /// This object class is used to retrieve a list of urls for objects to be returned to client
    /// </summary>
    public class GetObjectsModel
    {
        /// <summary>
        /// Gets or sets the list of urls
        /// </summary>
        public List<string> uris
        {
            get;
            set;
        }
    }
}

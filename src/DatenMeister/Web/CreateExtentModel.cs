using System;
namespace DatenMeister
{
    /// <summary>
    /// Contains the model, which is used to create a new extent.
    /// Corresponds to view 'datenmeister.views.CreateNewExtentView'
    /// </summary>
    public class CreateExtentModel
    {
        /// <summary>
        /// Gets or sets the name of the new extent
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the filename of the new extent. The filename should be without extension
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public string filename
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URL which is used by the extent
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string url
        {
            get;
            set;
        }
    }
}


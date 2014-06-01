using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.Application
{
    /// <summary>
    /// Stores the data that is used for the application specific data
    /// </summary>
    public class ApplicationData
    {
        /// <summary>
        /// Stores the application data in an extent
        /// </summary>
        private IURIExtent applicationData;

        /// <summary>
        /// Gets or sets the name of the project
        /// </summary>
        public string ProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the project
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        public ApplicationData(string projectName)
        {
            this.ProjectName = projectName;
        }
    }
}

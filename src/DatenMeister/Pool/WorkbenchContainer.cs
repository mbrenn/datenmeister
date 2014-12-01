using DatenMeister.Entities.DM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Pool
{
    /// <summary>
    /// The class just contains the workbench class and shall be attached as a singleton to
    /// the dependency framework
    /// </summary>
    public class WorkbenchContainer
    {
        /// <summary>
        /// Gets or sets the workbench
        /// </summary>
        public Workbench Workbench
        {
            get;
            set;
        }
    }
}

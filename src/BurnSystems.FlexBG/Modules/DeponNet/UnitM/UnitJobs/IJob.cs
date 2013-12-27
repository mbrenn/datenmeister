using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM.UnitJobs
{
    /// <summary>
    /// Common interface for all jobs
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// Gets or sets a value, indicating whether user has created the job or it has been created by engine
        /// </summary>
        bool IsUserDefined
        {
            get;
            set;
        }
    }
}

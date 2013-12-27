using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM.UnitJobs
{
    [Serializable]
    public class BaseJob : IJob
    {
        /// <summary>
        /// Stores the value whether the job is user defined
        /// </summary>
        private bool isUserDefined;

        /// <summary>
        /// Gets or sets a value indicating whether the job is userdefined
        /// </summary>
        public bool IsUserDefined
        {
            get { return this.isUserDefined; }
            set { this.isUserDefined = value; }
        }
    }
}

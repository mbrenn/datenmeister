using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.MapFieldOwnerM
{
    /// <summary>
    /// Defines the configuration for the algorithm
    /// </summary>
    public class AlgorithmConfig
    {
        /// <summary>
        /// Initializes a new instance of the AlgorithmConfig class.
        /// </summary>
        public AlgorithmConfig()
        {
            this.MaxRadius = 10;
        }

        /// <summary>
        /// Gets or sets the maximum radius
        /// </summary>
        public double MaxRadius
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data key being used to assign the owner
        /// </summary>
        public int DataKey
        {
            get;
            set;
        }
    }
}

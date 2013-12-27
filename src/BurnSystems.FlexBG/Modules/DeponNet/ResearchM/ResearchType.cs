using BurnSystems.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResearchM
{
    /// <summary>
    /// Enumerates and stores the possible field types
    /// </summary>
    public class ResearchType : IHasId
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Token that will be used for messages
        /// </summary>
        public string Token
        {
            get;
            set;
        }
    }
}

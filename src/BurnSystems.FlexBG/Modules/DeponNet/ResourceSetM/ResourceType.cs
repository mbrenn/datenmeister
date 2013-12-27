using BurnSystems.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM
{
    /// <summary>
    /// Defines the list of existing resource types. 
    /// Internally, the id is used
    /// </summary>
    public class ResourceType : IHasId
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
        /// Gets or sets the token
        /// </summary>
        public string Token
        {
            get;
            set;
        }
    }
}

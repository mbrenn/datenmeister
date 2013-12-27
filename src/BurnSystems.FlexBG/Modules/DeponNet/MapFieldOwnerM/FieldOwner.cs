using BurnSystems.FlexBG.Modules.DeponNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.MapFieldOwnerM
{
    /// <summary>
    /// Defines one entity that is located on the map and is requesting ownership for a certain field
    /// </summary>
    public class FieldOwner
    {
        /// <summary>
        /// Defines the id of the owner
        /// </summary>
        public long OwnerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position
        /// </summary>
        public ObjectPosition Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the influence by this object. 
        /// The bigger the influence is, the more fields will be owned
        /// </summary>
        public double Influence
        {
            get;
            set;
        }
    }
}

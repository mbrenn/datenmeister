using BurnSystems.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.BuildingM
{
    /// <summary>
    /// Defines the building type
    /// </summary>
    public class BuildingType : IHasId
    {
        /// <summary>
        /// Gets or sets the id of the buildung
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

        public int SizeX
        {
            get;
            set;
        }

        public int SizeY
        {
            get;
            set;
        }
    }
}

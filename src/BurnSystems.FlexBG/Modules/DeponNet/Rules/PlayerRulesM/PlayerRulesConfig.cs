using BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.PlayerRulesM
{
    /// <summary>
    /// Defines the player rules themselves
    /// </summary>
    public class PlayerRulesConfig
    {
        /// <summary>
        /// Gets the start resources for the player
        /// </summary>
        public ResourceSet PlayerStartResources
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the start resources for the first town
        /// </summary>
        public ResourceSet TownStartResources
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the PlayerRulesConfig class. 
        /// </summary>
        public PlayerRulesConfig()
        {
            this.PlayerStartResources = new ResourceSet();
            this.TownStartResources = new ResourceSet();
        }
    }
}

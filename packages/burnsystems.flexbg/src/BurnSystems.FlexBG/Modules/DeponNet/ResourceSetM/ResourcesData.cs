using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM
{
    /// <summary>
    /// Entites that are persisted
    /// </summary>
    [Serializable]
    public class ResourcesData
    {
        /// <summary>
        /// Stores the list of games
        /// </summary>
        private List<ResourceSetBag> resources = new List<ResourceSetBag>();

        /// <summary>
        /// Gets the games
        /// </summary>
        public List<ResourceSetBag> Resources
        {
            get { return this.resources; }
        }
    }
}

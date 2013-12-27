using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.BuildingM
{
    [Serializable]
    public class BuildingsData
    {
        /// <summary>
        /// Stores the list of games
        /// </summary>
        private List<Building> buildings = new List<Building>();

        /// <summary>
        /// Gets the games
        /// </summary>
        public List<Building> Buildings
        {
            get { return this.buildings; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.TownM
{
    [Serializable]
    public class TownsData
    {
        private List<Town> towns = new List<Town>();

        /// <summary>
        /// Gets the towns
        /// </summary>
        public List<Town> Towns
        {
            get { return this.towns; }
        }
    }
}

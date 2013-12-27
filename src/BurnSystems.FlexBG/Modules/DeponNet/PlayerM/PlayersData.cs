using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.PlayerM
{
    [Serializable]
    public class PlayersData
    {
        private List<Player> players = new List<Player>();

        /// <summary>
        /// Gets the players
        /// </summary>
        public List<Player> Players
        {
            get { return this.players; }
        }
    }
}

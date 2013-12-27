using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameClockM
{
    /// <summary>
    /// Stores the information
    /// </summary>
    [Serializable]
    public class GameClocksData 
    {
        /// <summary>
        /// Stores the game clock information
        /// </summary>
        private List<GameClockInfo> gameClockInfos = new List<GameClockInfo>();

        public List<GameClockInfo> GameClockInfos
        {
            get { return this.gameClockInfos; }
        }
    }
}

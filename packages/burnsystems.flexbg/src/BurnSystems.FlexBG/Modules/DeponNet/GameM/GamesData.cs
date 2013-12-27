using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM
{
    /// <summary>
    /// Entites that are persisted
    /// </summary>
    [Serializable]
    public class GamesData
    {
        /// <summary>
        /// Stores the list of games
        /// </summary>
        private List<Game> games = new List<Game>();

        /// <summary>
        /// Gets the games
        /// </summary>
        public List<Game> Games
        {
            get { return this.games; }
        }
    }
}

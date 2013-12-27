using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM
{
    [Serializable]
    public class Game
    {
        /// <summary>
        /// Gets or sets the game id
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the game is paused
        /// </summary>
        public bool IsPaused
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum player count
        /// </summary>
        public int MaxPlayers
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Title;
        }

        public object AsJson()
        {
            return new
            {
                title = this.Title,
                maxPlayers = this.MaxPlayers,
                isPaused = this.IsPaused,
                id = this.Id,
                description = this.Description
            };
        }
    }
}

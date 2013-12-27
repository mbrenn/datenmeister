using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM.Admin
{
    /// <summary>
    /// Creates the game for Depon.Net
    /// </summary>
    public class DeponCreateGameModel
    {
        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int MaxPlayers
        {
            get;
            set;
        }

        public int MapWidth
        {
            get;
            set;
        }

        public int MapHeight
        {
            get;
            set;
        }
    }
}

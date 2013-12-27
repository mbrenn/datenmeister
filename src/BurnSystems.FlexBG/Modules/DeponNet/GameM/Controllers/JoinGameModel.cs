using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers
{
    public class JoinGameModel
    {
        public string Playername
        {
            get;
            set;
        }

        public string Empirename
        {
            get;
            set;
        }

        public string Townname
        {
            get;
            set;
        }

        public long GameId
        {
            get;
            set;
        }
    }
}

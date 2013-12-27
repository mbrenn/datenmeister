using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.PlayerRulesM
{
    public class PlayerCreationParams
    {
        public long GameId
        {
            get;
            set;
        }

        public long UserId
        {
            get;
            set;
        }

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

        public string FirstTownName
        {
            get;
            set;
        }
    }
}

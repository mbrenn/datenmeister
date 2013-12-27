using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM.Strategies
{
    public enum UnitStrategyType
    {
        Passive,
        Aggressive
    }

    [Serializable]
    public class UnitStrategy
    {
        public UnitStrategyType Type
        {
            get;
            set;
        }

        public double AttackRadius
        {
            get;
            set;
        }

        public double FollowRadius
        {
            get;
            set;
        }
    }
}

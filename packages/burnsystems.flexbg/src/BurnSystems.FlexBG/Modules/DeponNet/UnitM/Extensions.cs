using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM
{
    public static class Extensions
    {
        public static IEnumerable<Unit> GetUnitsOnField(this IUnitDataProvider provider, int x, int y)
        {
            return provider.GetUnitsInRegion(
                x,
                y,
                x + 1,
                y + 1);
        }
    }
}

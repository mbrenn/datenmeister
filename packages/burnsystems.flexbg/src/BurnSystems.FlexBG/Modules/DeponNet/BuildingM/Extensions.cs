using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.BuildingM
{
    public static class Extensions
    {
        /// <summary>
        /// Get all buildings on a certain field.
        /// </summary>
        /// <param name="playerId">Id of the player</param>
        /// <param name="x">X-Coordinate of the field</param>
        /// <param name="y">Y-Coordinate of the field</param>
        /// <returns>Enumeration of buildings</returns>
        public static IEnumerable<Building> GetBuildingsOnField(this IBuildingDataProvider provider, int x, int y)
        {
            return provider.GetBuildingsInRegion ( 
                x,
                y, 
                x + 1,
                y+ 1);
        }
    }
}

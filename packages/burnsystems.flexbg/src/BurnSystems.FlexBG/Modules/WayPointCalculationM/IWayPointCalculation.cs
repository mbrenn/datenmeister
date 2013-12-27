using BurnSystems.FlexBG.Modules.DeponNet.Common;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.WayPointCalculationM
{
    /// <summary>
    /// Interface for waypoint calculation
    /// </summary>
    public interface IWayPointCalculation
    {
        /// <summary>
        /// Calculates the waypoints for the given start and end position
        /// </summary>
        /// <param name="startPosition">Position, where unit currently is</param>
        /// <param name="endPosition">Position, where unit will be</param>
        /// <param name="unitType">Type of the unit which shall be calculated</param>
        /// <returns>Enumeration of waypoints</returns>
        IEnumerable<ObjectPosition> CalculateWaypoints(ObjectPosition startPosition, ObjectPosition endPosition, UnitType unitType);
    }
}

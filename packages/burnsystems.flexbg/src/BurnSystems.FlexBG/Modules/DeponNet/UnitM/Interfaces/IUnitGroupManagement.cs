using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces
{
    /// <summary>
    /// Defines the interface to access unit groups
    /// </summary>
    public interface IUnitGroupManagement
    {
        long CreateUnitGroup(long playerId);

        void DissolveUnitGroup();

        void AddUnit(long unitGroupId, long unitId);

        void RemoveUnit(long unitGroupId, long unitId);

        UnitGroup Get(long unitGroupId);

        IEnumerable<UnitGroup> GetAllUnitGroups();

        IEnumerable<UnitGroup> GetUnitGroupsOfPlayer(long playerId);

        IEnumerable<Unit> GetUnits(long unitGroupId);
    }
}

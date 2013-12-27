using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM
{
    public class UnitGroupManagement : IUnitGroupManagement
    {
        public long CreateUnitGroup(long playerId)
        {
            throw new NotImplementedException();
        }

        public void DissolveUnitGroup()
        {
            throw new NotImplementedException();
        }

        public void AddUnit(long unitGroupId, long unitId)
        {
            throw new NotImplementedException();
        }

        public void RemoveUnit(long unitGroupId, long unitId)
        {
            throw new NotImplementedException();
        }

        public UnitGroup Get(long unitGroupId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UnitGroup> GetAllUnitGroups()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UnitGroup> GetUnitGroupsOfPlayer(long playerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Unit> GetUnits(long unitGroupId)
        {
            throw new NotImplementedException();
        }
    }
}

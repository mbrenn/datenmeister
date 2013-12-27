using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data
{
    [Serializable]
    public class UnitsData
    {
        private List<Unit> units = new List<Unit>();

        private List<UnitGroup> unitGroups = new List<UnitGroup>();

        public List<Unit> Units
        {
            get { return this.units; }
        }

        public List<UnitGroup> UnitGroups
        {
            get { return this.unitGroups; }
        }
    }
}

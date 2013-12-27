using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data
{
    [Serializable]
    public class UnitGroup
    {
        private long id;
        private long ownerId;

        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public long OwnerId
        {
            get { return this.ownerId; }
            set { this.ownerId = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResearchM
{
    [Serializable]
    public class Research
    {
        private long id;

        private long ownerId;

        private long researchTypeId;

        private int level;

        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public long PlayerId
        {
            get { return this.ownerId; }
            set { this.ownerId = value; }
        }

        public long ResearchTypeId
        {
            get { return this.researchTypeId; }
            set { this.researchTypeId = value; }
        }

        public int Level
        {
            get { return this.level; }
            set { this.level = value; }
        }
    }
}

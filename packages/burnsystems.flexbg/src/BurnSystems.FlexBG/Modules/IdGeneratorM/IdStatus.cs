using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.IdGeneratorM
{
    [Serializable]
    public class IdStatus
    {
        /// <summary>
        /// Stores the ids
        /// </summary>
        private Dictionary<int, long> ids = new Dictionary<int, long>();

        /// <summary>
        /// Gets the ids
        /// </summary>
        public Dictionary<int, long> Ids
        {
            get { return this.ids; }
        }
    }
}

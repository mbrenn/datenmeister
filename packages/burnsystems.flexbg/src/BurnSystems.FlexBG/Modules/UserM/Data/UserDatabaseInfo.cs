using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Data
{
    [Serializable]
    public class UserDatabaseInfo
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the id of the last group
        /// </summary>
        protected long lastGroupId = 0;

        /// <summary>
        /// Stores the id of the last user
        /// </summary>
        protected long lastUserId = 0;

        public long LastGroupId
        {
            get { return this.lastGroupId; }
            set { this.lastGroupId = value; }
        }

        public long LastUserId
        {
            get { return this.lastUserId; }
            set { this.lastUserId = value; }
        }
    }
}

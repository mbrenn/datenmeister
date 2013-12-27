using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Models
{
    /// <summary>
    /// Stores the memberships
    /// </summary>
    [Serializable]
    public class Membership
    {
        private long id;
        private long userId;
        private long groupId;

        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Id of the user
        /// </summary>
        public long UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        /// <summary>
        /// Id of the group
        /// </summary>
        public long GroupId
        {
            get { return this.groupId; }
            set { this.groupId = value; }
        }
    }
}

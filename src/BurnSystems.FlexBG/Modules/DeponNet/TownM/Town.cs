using BurnSystems.FlexBG.Modules.DeponNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.TownM
{
    /// <summary>
    /// Defines the town
    /// </summary>
    [Serializable]
    public class Town
    {
        private long id;
        private long ownerId;
        private string townName;
        private bool isCapital;
        private ObjectPosition position;

        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets the id of the owner
        /// </summary>
        public long OwnerId
        {
            get { return this.ownerId; }
            set { this.ownerId = value; }
        }

        /// <summary>
        /// Gets or sets the town name
        /// </summary>
        public string TownName
        {
            get { return this.townName; }
            set { this.townName = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the town is a capital town
        /// </summary>
        public bool IsCapital
        {
            get { return this.isCapital; }
            set { this.isCapital = true; }
        }

        /// <summary>
        /// Gets the position of the town
        /// </summary>
        public ObjectPosition Position
        {
            get { return this.position; }
            set { this.position = value; }
        }
    }
}

using BurnSystems.FlexBG.Modules.DeponNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.BuildingM
{
    [Serializable]
    public class Building
    {
        private long id;

        private long townId;

        private long playerId;

        private long buildingTypeId;

        private int level;

        private bool isActive;

        private double productivity;

        private ObjectPosition position = new ObjectPosition();

        /// <summary>
        /// Gets or sets the id of the building
        /// </summary>
        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets the id of the town, where building is associated
        /// </summary>
        public long TownId
        {
            get { return this.townId; }
            set { this.townId = value; }
        }

        /// <summary>
        /// Gets or sets the id of the player, who is owner of the building
        /// </summary>
        public long PlayerId
        {
            get { return this.playerId; }
            set { this.playerId = value; }
        }

        /// <summary>
        /// Stores the building type
        /// </summary>
        public long BuildingTypeId
        {
            get { return this.buildingTypeId; }
            set { this.buildingTypeId = value; }
        }

        /// <summary>
        /// Gets or sets level of building
        /// </summary>
        public int Level
        {
            get { return this.level; }
            set { this.level = value; }
        }

        /// <summary>
        /// Gets or sets information whether the building is currently active
        /// </summary>
        public bool IsActive
        {
            get { return this.isActive; }
            set { this.isActive = value; }
        }

        /// <summary>
        /// Gets or sets the productivity of the building. 
        /// Can be a number between 0.0 and 1.0
        /// </summary>
        public double Productivity
        {
            get { return this.productivity; }
            set { this.productivity = value; }
        }

        public ObjectPosition Position
        {
            get
            {
                if (this.position == null)
                {
                    this.position = new ObjectPosition();
                }

                return this.position;
            }
            set { this.position = value; }
        }
    }
}

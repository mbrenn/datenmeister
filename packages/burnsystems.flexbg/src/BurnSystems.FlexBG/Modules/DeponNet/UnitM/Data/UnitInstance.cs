using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data
{
    /// <summary>
    /// Defines one single instance of the unit
    /// </summary>
    [Serializable]
    public class UnitInstance
    {
        /// <summary>
        /// Stores the id
        /// </summary>
        private long id;

        /// <summary>
        /// Stores a value indicating whether the unit is dead
        /// </summary>
        private bool isDead;

        /// <summary>
        /// Stores the lifepoints of the unit
        /// </summary>
        private double lifePoints;
        
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the unit is dead
        /// </summary>
        public bool IsDead
        {
            get { return this.isDead; }
            set { this.isDead = value; }
        }

        /// <summary>
        /// Gets or sets the life points of the unit
        /// </summary>
        public double LifePoints
        {
            get { return this.lifePoints; }
            set { this.lifePoints = value; }
        }
    }
}

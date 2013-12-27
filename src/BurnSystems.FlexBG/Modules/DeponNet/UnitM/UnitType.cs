using BurnSystems.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM
{
    /// <summary>
    /// Defines the unit types
    /// </summary>
    public class UnitType : IHasId
    {
        /// <summary>
        /// Gets or sets the id of the Unit type
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the token
        /// </summary>
        public string Token
        {
            get;
            set;
        }

        public double Velocity
        {
            get;
            set;
        }

        public double LifePoints
        {
            get;
            set;
        }

        public double AttackPoints
        {
            get;
            set;
        }

        public double DefensePoints
        {
            get;
            set;
        }
    }
}

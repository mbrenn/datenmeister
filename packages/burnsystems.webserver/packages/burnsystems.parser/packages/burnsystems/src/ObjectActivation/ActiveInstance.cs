using System;
using System.Collections.Generic;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// Stores the instance and the criterias
    /// that has been queried before and shall be reused.
    /// </summary>
    internal class ActiveInstance
    {
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of criteria being used to create this instance. 
        /// </summary>
        public List<CriteriaCatalogue> Criterias
        {
            get;
            set;
        }

        public override string ToString()
        {
            if (this.Value == null)
            {
                return "-> null";
            }

            return "-> " + this.Value.ToString();
        }
    }
}

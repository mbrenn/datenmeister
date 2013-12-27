using System;
using BurnSystems.ObjectActivation.Enabler;

namespace BurnSystems.ObjectActivation.Criteria
{
    /// <summary>
    /// This criteria checks, if the enabler contains the correct object
    /// </summary>
    public class ByTypeCriteria : ICriteria
    {
        /// <summary>
        /// Gets the type 
        /// </summary>
        public Type Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the ByType class.
        /// </summary>
        /// <param name="type">Type of the activation</param>
        public ByTypeCriteria(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            this.Type = type;
        }

        /// <summary>
        /// Converts object to string
        /// </summary>
        /// <returns>Converted object</returns>
        public override string ToString()
        {
            return string.Format("[ByType Type={0}]", Type.ToString());
        }

        /// <summary>
        /// Returns true, of object matches to enabler
        /// </summary>
        /// <param name="enabler">Enabler to be checked</param>
        /// <returns>true, if we have a match</returns>						
        public bool DoesMatch(IEnabler enabler)
        {
            var value = enabler as ByTypeEnabler;
            if (value != null && value.Type == this.Type)
            {
                return true;
            }

            return false;
        }
    }
}

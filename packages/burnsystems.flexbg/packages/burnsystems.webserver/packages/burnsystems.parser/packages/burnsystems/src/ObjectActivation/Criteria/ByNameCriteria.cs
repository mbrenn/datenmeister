using System;
using BurnSystems.ObjectActivation.Enabler;

namespace BurnSystems.ObjectActivation.Criteria
{
    /// <summary>
    /// This criteria enables the activation, if 
    /// a given string matches to enabler.
    /// </summary>
    public class ByNameCriteria : ICriteria
    {
        /// <summary>
        /// Gets the name of the criteria
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the ByName class.
        /// </summary>
        /// <param name="name">Name of the Enabler</param>
        public ByNameCriteria(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            this.Name = name;
        }

        /// <summary>
        /// Converts object to string
        /// </summary>
        /// <returns>Converted object</returns>
        public override string ToString()
        {
            return string.Format("[ByName Type={0}]", Name.ToString());
        }

        /// <summary>
        /// Returns true, of object matches to enabler
        /// </summary>
        /// <param name="enabler">Enabler to be checked</param>
        /// <returns>true, if we have a match</returns>
        public bool DoesMatch(IEnabler enabler)
        {
            var value = enabler as ByNameEnabler;
            if (value != null && value.Name == this.Name)
            {
                return true;
            }

            return false;
        }
    }
}

using System;

namespace BurnSystems.ObjectActivation.Enabler
{
    /// <summary>
    /// This enabler is required for ByName criteria and
    /// matches only if the name matches to the criteria
    /// </summary>
    public class ByNameEnabler : IEnabler
    {
        /// <summary>
        /// Gets the name of the enabler
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the ByName class.
        /// </summary>
        /// <param name="name">Name of the class</param>
        public ByNameEnabler(string name)
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
    }
}

using System;

namespace BurnSystems.ObjectActivation.Enabler
{
    /// <summary>
    /// This enabler contains the type of the object to be created.
    /// </summary>
    public class ByTypeEnabler : IEnabler
    {
        /// <summary>
        /// Gets the type of the enabler
        /// </summary>
        public Type Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the ByType class.
        /// </summary>
        /// <param name="type">Type of the enabler</param>
        public ByTypeEnabler(Type type)
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
    }
}

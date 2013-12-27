using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// Properties having this attribute get automatically injected, if possible
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Parameter, Inherited = false, AllowMultiple = true)]
    public sealed class InjectAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether injection shall be executed by the object
        /// itself by using InstanceBuilder
        /// </summary>
        public bool ByItself
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the injection
        /// </summary>
        public string ByName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the injection is mandatory
        /// </summary>
        public bool IsMandatory
        {
            get;
            set;
        }

        /// <summary>
        /// Performs the injection by type
        /// </summary>
        public InjectAttribute()
        {
        }

        /// <summary>
        /// Performs the injection by name instead of by type
        /// </summary>
        /// <param name="byName"></param>
        public InjectAttribute(string byName)
        {
            this.ByName = byName;
        }
    }
}

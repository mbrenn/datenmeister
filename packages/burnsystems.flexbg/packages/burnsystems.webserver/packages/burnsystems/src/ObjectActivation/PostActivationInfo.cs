
using System;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// This class stores which object and how the object
    /// had been activated. This class is created to execute the required 
    /// actions dependend on the activation conditions. 
    /// </summary>
    internal class PostActivationInfo
    {
        /// <summary>
        /// Gets the information about the associated activation info
        /// </summary>
        public ActivationInfo ActivationInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the information about the activated object
        /// </summary>
        public object ActivatedObject
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the PostActivationInfo class
        /// </summary>
        /// <param name="activatedObject">The object that has been created</param>
        /// <param name="activationInfo">The activationinfo being associated.</param>
        public PostActivationInfo(object activatedObject, ActivationInfo activationInfo)
        {
            if (activatedObject == null)
                throw new ArgumentNullException("activatedObject");
            if (activationInfo == null)
                throw new ArgumentNullException("activationInfo");

            this.ActivatedObject = activatedObject;
            this.ActivationInfo = activationInfo;
        }
    }
}

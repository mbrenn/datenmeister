using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// This exception is thrown when a problem with ObjectActivation had been found
    /// </summary>
    [Serializable]
    public class ObjectActivationException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the ObjectActivationException
        /// </summary>
        public ObjectActivationException() { }

        /// <summary>
        /// Initializes a new instance of the ObjectActivationException
        /// </summary>
        /// <param name="message">Message being sent out</param>
        public ObjectActivationException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the ObjectActivationException
        /// </summary>
        /// <param name="message">Message being sent out</param>
        /// <param name="inner">Inner Exception</param>
        public ObjectActivationException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the ObjectActivationException
        /// </summary>
        /// <param name="info">Information being used</param>
        /// <param name="context">Context being used</param>
        protected ObjectActivationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

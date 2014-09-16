using BurnSystems.ObjectActivation;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    /// <summary>
    /// Defines the injection helper
    /// </summary>
    public static class Injection
    {
        /// <summary>
        /// Stores the activation container on application scope
        /// </summary>
        private static IKernel application;

        /// <summary>
        /// Returns the activation container on application scope. 
        /// The variable has to be initialized via InitForApplication()
        /// </summary>
        public static IKernel Application
        {
            get
            {
                if (application == null)
                {
                    InitForApplication();
                }

                return application;
            }
        }

        /// <summary>
        /// Initializes the activation container on application scope
        /// </summary>
        private static void InitForApplication()
        {
            if (application == null)
            {
                application = new StandardKernel();
            }
        }

        /// <summary>
        /// Deletes the activattion container and creates a complete new instance. 
        /// This is necessary for unit testing for example
        /// </summary>
        public static void Reset()
        {
            application = null;
        }
    }
}

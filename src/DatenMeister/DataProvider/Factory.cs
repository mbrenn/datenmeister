using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// Implements the IFactory class in a very simple way by just calling 
    /// IURIExtent.CreateObject
    /// </summary>
    public  abstract class Factory : IFactory
    {
        public Factory()
        {
        }

        [Obsolete("Use Factory()")]
        public Factory(IURIExtent extent)
        {   
        }

        public abstract IObject create(IObject type);

        /// <summary>
        /// Creates am object by an object and its given datatype
        /// </summary>
        /// <param name="dataType">Datatype to be used</param>
        /// <param name="value">Value to be used</param>
        /// <returns>Created object</returns>
        public IObject createFromString(IObject dataType, string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <param name="dataType">Datatype of the object</param>
        /// <param name="value">Object to be converted to a string</param>
        /// <returns>String representation of the object</returns>
        public string convertToString(IObject dataType, IObject value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the factory for a certain event by using the default binder
        /// </summary>
        /// <param name="extent">Extent, whose factory is requested</param>
        /// <returns>Factory to be created</returns>
        public static IFactory GetFor(IURIExtent extent)
        {
            var factoryProvider = Global.Application.Get<IFactoryProvider>();
            return factoryProvider.CreateFor(extent);
        }
    }
}
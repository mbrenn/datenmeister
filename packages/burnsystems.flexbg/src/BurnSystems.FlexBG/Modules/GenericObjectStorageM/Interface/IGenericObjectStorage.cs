using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.GenericObjectStorageM.Interface
{
    /// <summary>
    /// Defines a generic storage that can be used to store data without implementing your own storage handler
    /// </summary>
    public interface IGenericObjectStorage
    {
        /// <summary>
        /// Sets by path
        /// </summary>
        /// <typeparam name="T">Type to be added</typeparam>
        /// <param name="path">Path to be used</param>
        void Set<T>(string path, T value) where T : class;

        /// <summary>
        /// Gets the information by path
        /// </summary>
        /// <typeparam name="T">Type to be retrieved</typeparam>
        /// <param name="path">Path to be used</param>
        /// <returns>Retrieved object</returns>
        T Get<T>(string path) where T : class;
    }
}

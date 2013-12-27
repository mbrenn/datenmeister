using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.IdGeneratorM
{
    /// <summary>
    /// Generates the id for given entity types.
    /// The generated ids are stored, if FlexBG is closed
    /// </summary>
    public interface IIdGenerator
    {
        /// <summary>
        /// Returns the id for the given entity type. 
        /// This entity type is thread safe.
        /// </summary>
        /// <param name="entityType">Type of the entity</param>
        /// <returns></returns>
        long NextId(int entityType);
    }
}

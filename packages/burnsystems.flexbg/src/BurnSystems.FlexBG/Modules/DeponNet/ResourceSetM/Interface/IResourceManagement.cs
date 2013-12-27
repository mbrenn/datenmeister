using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM.Interface
{
    /// <summary>
    /// Defines the interface for resources
    /// </summary>
    public interface IResourceManagement
    {
        /// <summary>
        /// Gets the resource set 
        /// </summary>
        /// <param name="entityType">Type of the entity</param>
        /// <param name="entityId">Id of the entity</param>
        /// <returns>Found resources</returns>
        ResourceSetBag GetResources(int entityType, long entityId);

        /// <summary>
        /// Sets the resource set
        /// </summary>
        /// <param name="entityType">Type of the entity</param>
        /// <param name="entityId">Id of the entity</param>
        /// <param name="resources">Resources to be set</param>
        void SetAvailable(int entityType, long entityId, ResourceSet resources);

        /// <summary>
        /// Converts the resourceset to json object
        /// </summary>
        /// <param name="resources">Resources to be converted</param>
        /// <returns>Converted json object</returns>
        object AsJson(ResourceSetBag resources);
    }
}

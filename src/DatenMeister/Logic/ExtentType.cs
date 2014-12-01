using System;
namespace DatenMeister.Logic
{
    /// <summary>
    /// Defines the possible extent types
    /// </summary>
    public enum ExtentType
    {
        /// <summary>
        /// Defines the extent type, when the given type is unknown
        /// </summary>
        Unknown, 

        /// <summary>
        /// Defines that the given extent contains all extents
        /// </summary>
        Extents, 

        /// <summary>
        /// Defines the types necessary to execute the application itself
        /// </summary>
        MetaType,
        
        /// <summary>
        /// Types of the application itself
        /// </summary>
        Type,

        /// <summary>
        /// Views of the application
        /// </summary>
        View, 

        /// <summary>
        /// Data of the current project
        /// </summary>
        Data,
        
        /// <summary>
        /// Data for the application being used to. It is independent of any loaded workset
        /// </summary>
        ApplicationData,

        /// <summary>
        /// Queries which contain a filtered, sorted or any other type of extent being dependent on one
        /// of the extents above
        /// </summary>
        Query
    }
}


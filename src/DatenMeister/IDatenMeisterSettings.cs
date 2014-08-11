using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DatenMeister
{
    /// <summary>
    /// Defines the additional settings, that shall only be accessed via
    /// the ApplicationCore helper methods
    /// </summary>
    public interface IDatenMeisterSettings : IPublicDatenMeisterSettings
    {
        /// <summary>
        /// Performs the full initialization at application start. 
        /// This method is just called once
        /// </summary>
        void InitializeForBootUp(ApplicationCore core);

        /// <summary>
        /// This function will be called, when a new viewset needs to be created. 
        /// It is independent to the fact whether the containing extents and viewinformation
        /// is loaded or is created from Scratch.
        /// </summary>
        void InitializeViewSet(ApplicationCore core);

        /// <summary>
        /// The function will be called, when the user wants to have an extent/viewset from
        /// scratch. This means, that he has clicked "File->New"
        /// </summary>
        void InitializeFromScratch(ApplicationCore core);

        /// <summary>
        /// The function will be called, when application has been started. 
        /// It can be used to include some example data
        /// </summary>
        void InitializeForExampleData(ApplicationCore core);

        /// <summary>
        /// The function will be called, before the application gets closed.
        /// It can be used to store the latest changes. 
        /// </summary>
        /// <param name="core">Core to be used</param>
        void StoreViewSet(ApplicationCore core);
    }
}

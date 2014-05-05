using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.Views
{
    /// <summary>
    /// Enumerates the possible view types
    /// </summary>
    public enum ViewType
    {
        /// <summary>
        /// View is a table
        /// </summary>
        TableView, 

        /// <summary>
        /// View is a form
        /// </summary>
        FormView
    }

    public interface IViewManager
    {
        /// <summary>
        /// Gets default view
        /// </summary>
        /// <param name="obj">Object to be evaluated</param>
        /// <param name="type">Type of the view, which is required</param>
        /// <returns>The default view or null, if not existing</returns>
        IObject GetDefaultView(IObject obj, ViewType type);

        /// <summary>
        /// Gets the views, which are applicable for the item 
        /// </summary>
        /// <param name="obj">Object to be evaulated</param>
        /// <param name="type">Type of the view, which is required</param>
        /// <returns>An enumeration of all applicable view definitions. Might also be empty</returns>
        IEnumerable<IObject> GetViews(IObject obj, ViewType type);
    }
}

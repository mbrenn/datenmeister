using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.Treeview
{
    /// <summary>
    /// Identifies one treeview item. Can be used as ModelView-Item
    /// </summary>
    public interface ITreeViewItem
    {
        /// <summary>
        /// Gets or sets the id of the item
        /// </summary>
        long Id
        {
            get;
        }

        /// <summary>
        /// Gets the title
        /// </summary>
        string Title
        {
            get;
        }

        /// <summary>
        /// Gets or sets the image url
        /// </summary>
        string ImageUrl
        {
            get;
        }

        /// <summary>
        /// Gets or sets the value indicating whether the current item is expandable
        /// </summary>
        bool IsExpandable
        {
            get;
        }

        /// <summary>
        /// Gets or sets the entity behind this treeview object
        /// </summary>
        object Entity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the children
        /// </summary>
        /// <param name="container">Container to be used</param>
        IEnumerable<ITreeViewItem> GetChildren(IActivates container);

        /// <summary>
        /// Applies changes of the object back to the database
        /// </summary>
        /// <param name="container">Container being used to retrieve information</param>
        /// <param name="entity">Entity to be called</param>
        void ApplyChanges(IActivates container);
    }
}

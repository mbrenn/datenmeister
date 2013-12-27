using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.DetailView.Entities
{
    /// <summary>
    /// Defines the table that can be shown in an entity view. 
    /// Can be a Detail or a List table
    /// </summary>
    public abstract class EntityViewTable
    {
        /// <summary>
        /// Converts the thing to json
        /// </summary>
        /// <param name="item">Item to be used</param>
        /// <returns>Converted object</returns>
        public abstract object ToJson(IActivates container, ITreeViewItem item);
    }
}

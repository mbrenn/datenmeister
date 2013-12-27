using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Umbra.Requests;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.DetailView
{
    /// <summary>
    /// Performs the execution of the detail view request.
    /// An instance will be created for each request
    /// </summary>
    public abstract class DetailView : BaseUmbraRequest
    {
        /// <summary>
        /// Gets or sets the item to be shown as detail view
        /// </summary>
        public ITreeViewItem Item
        {
            get;
            set;
        }
    }
}

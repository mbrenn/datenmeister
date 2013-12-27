using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.DetailView
{
    /// <summary>
    /// Performs the resolving of items to IDetailView implementations
    /// </summary>
    public interface IDetailViewResolver
    {
        /// <summary>
        /// Resolves default view for given object
        /// </summary>
        /// <param name="item">Item to be shown</param>
        /// <returns>Resolved default view type or null, if not existing</returns>
        DetailView ResolveDefaultView(IActivates container, ITreeViewItem item);
    }
}

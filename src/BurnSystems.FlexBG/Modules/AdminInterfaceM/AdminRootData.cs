using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.AdminInterfaceM
{
    /// <summary>
    /// Root object for data
    /// </summary>
    public class AdminRootData : BaseTreeViewItem
    {
        /// <summary>
        /// Stores the children
        /// </summary>
        private List<ITreeViewItem> children = new List<ITreeViewItem>();

        /// <summary>
        /// Stores the name 
        /// </summary>
        public const string Name = "AdminRootData";

        /// <summary>
        /// Gets the unique id
        /// </summary>
        public override long Id
        {
            get { return 1; }
        }

        /// <summary>
        /// Gets the title
        /// </summary>
        public override string Title
        {
            get { return "FlexBG"; }
        }

        /// <summary>
        /// Gets the children
        /// </summary>
        public IList<ITreeViewItem> Children
        {
            get { return this.children; }
        }

        /// <summary>
        /// Gets the next unique id for children that can be used
        /// </summary>
        /// <returns>Next children id</returns>
        public long GetNextChildrenId()
        {
            return this.Children.Count + 1;
        }

        public override IEnumerable<ITreeViewItem> GetChildren(IActivates activates)
        {
            return this.children;
        }

        public override string ToString()
        {
            return "FlexBG Interface";
        }
    }
}

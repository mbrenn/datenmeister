using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.Treeview
{
    /// <summary>
    /// Base class for TreeViewItems. Can be used
    /// to derive your own tree view items
    /// </summary>
    public class BaseTreeViewItem : ITreeViewItem
    {
        private List<ITreeViewItem> children = new List<ITreeViewItem>();

        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public virtual long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the title to be shown in tree view
        /// </summary>
        public virtual string Title
        {
            get { return this.ToString(); }
        }

        /// <summary>
        /// Gets or sets the url for image
        /// </summary>
        public virtual string ImageUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value whether the item is expandable
        /// </summary>
        public virtual bool IsExpandable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the entity behind this treeview object
        /// </summary>
        public virtual object Entity
        {
            get;
            set;
        }

        /// <summary>
        /// Converts the item to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.Entity != null)
            {
                return this.Entity.ToString();
            }

            return typeof(BaseTreeViewItem).FullName;
        }

        /// <summary>
        /// Gets the children
        /// </summary>
        public virtual IEnumerable<ITreeViewItem> GetChildren(IActivates activates)
        {
            return null;
        }

        public virtual void ApplyChanges(IActivates container)
        {
            throw new NotImplementedException("BaseTreeViewItem.ApplyChanges");
        }
    }
}

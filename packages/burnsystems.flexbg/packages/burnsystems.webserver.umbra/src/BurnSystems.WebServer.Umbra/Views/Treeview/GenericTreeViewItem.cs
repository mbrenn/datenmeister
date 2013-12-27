using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.Treeview
{
    public class GenericTreeViewItem : ITreeViewItem
    {
        private List<ITreeViewItem> children = new List<ITreeViewItem>();

        public long Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string ImageUrl
        {
            get;
            set;
        }

        public bool IsExpandable
        {
            get;
            set;
        }

        public Action<IActivates> ApplyChangeFunction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the entity behind this treeview object
        /// </summary>
        public object Entity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the children
        /// </summary>
        public IList<ITreeViewItem> GetChildren(IActivates activates)
        {
            return this.children;
        }

        public virtual void ApplyChanges(IActivates container)
        {
            if (this.ApplyChangeFunction == null)
            {
                throw new NotImplementedException("BaseTreeViewItem.ApplyChanges");
            }
            else
            {
                this.ApplyChangeFunction(container);
            }
        }

        IEnumerable<ITreeViewItem> ITreeViewItem.GetChildren(IActivates activates)
        {
            return this.children;
        }

        public GenericTreeViewItem()
        {
        }

        public GenericTreeViewItem(long id, string title)
        {
            this.Id = id;
            this.Title = title;
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}

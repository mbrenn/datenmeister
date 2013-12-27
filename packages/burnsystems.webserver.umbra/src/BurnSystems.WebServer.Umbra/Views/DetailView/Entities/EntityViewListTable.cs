using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.DetailView.Entities
{
    /// <summary>
    /// Gets the list table
    /// </summary>
    public class EntityViewListTable<T> : EntityViewTable where T : class
    {
        /// <summary>
        /// Gets or sets the name of the table
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selector for the children of the treeview item
        /// </summary>
        public Func<IActivates, T, IEnumerable<object>> SelectorChildren
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rows
        /// </summary>
        public List<EntityViewElement> Elements
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the table
        /// </summary>
        /// <param name="name">Stores the name of the table</param>
        public EntityViewListTable(
            string name,
            params EntityViewElement[] elements)
        {
            this.Name = name;
            this.Elements = new List<EntityViewElement>();

            foreach (var element in elements)
            {
                this.AddElement(element);
            }
        }

        /// <summary>
        /// Adds a element
        /// </summary>
        /// <param name="element">Element to be added</param>
        /// <returns>Same instance</returns>
        public EntityViewListTable<T> AddElement(EntityViewElement element)
        {
            this.Elements.Add(element);
            return this;
        }

        /// <summary>
        /// Sets the selector
        /// </summary>
        /// <param name="selectorChildren"></param>
        /// <returns></returns>
        public EntityViewListTable<T> SetSelector(Func<IActivates, T, IEnumerable<object>> selectorChildren)
        {
            this.SelectorChildren = selectorChildren;
            return this;
        }

        /// <summary>
        /// Converts the object to a json object
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override object ToJson(IActivates container, Treeview.ITreeViewItem item)
        {
            Ensure.That(this.SelectorChildren != null, "No SelectorChildren set");

            var itemTyped = item as T;
            Ensure.That(itemTyped != null, "Item of type '" + item.GetType().FullName + "' could not be typed to '" + typeof(T).FullName + "'");

            return new
            {
                type = "list",
                elements = this.Elements.Select(x => x.ToJson(container)),
                data = this.SelectorChildren(container, itemTyped)
                    .Select(
                        child =>
                            this.Elements
                                .Select(element => element.ObjectToJson(child)))
            };
        }
    }
}

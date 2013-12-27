using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.DetailView
{
    /// <summary>
    /// Just a very simple resolver
    /// </summary>
    [BindAlsoTo(typeof(IDetailViewResolver))]
    public class BasicDetailViewResolver : IDetailViewResolver
    {
        /// <summary>
        /// Stores the list of items
        /// </summary>
        private List<Item> items = new List<Item>();

        /// <summary>
        /// Adds one item
        /// </summary>
        /// <param name="filter">Filter to be added</param>
        /// <param name="type">Type being used</param>
        public void Add(
            Func<ITreeViewItem, bool> filter,
            Type type)
        {
            this.items.Add(
                new Item()
                {
                    Filter = filter,
                    DetailViewType = type
                });
        }

        /// <summary>
        /// Adds one item and connects a factory method to the filter
        /// </summary>
        /// <param name="filter">Fitler to be added</param>
        /// <param name="factoryMethod">Factory method to be executed, if filter matches</param>
        public void Add(
            Func<ITreeViewItem, bool> filter,
            Func<IActivates, DetailView> factoryMethod)
        {
            this.items.Add(
                new Item()
                {
                    Filter = filter,
                    Factory = factoryMethod
                });
        }

        /// <summary>
        /// Resolves the type
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DetailView ResolveDefaultView(IActivates container, ITreeViewItem item)
        {
            var found = this.items.FirstOrDefault(x => x.Filter(item));
            if (found == null)
            {
                return null;
            }

            if (found.DetailViewType != null)
            {
                var result = container.Create(found.DetailViewType) as DetailView;
                Ensure.IsNotNull(result, "Could not create " + found.DetailViewType + " as DetailView");

                return result;
            }

            if (found.Factory != null)
            {
                return found.Factory(container);
            }

            Ensure.Fail("Could not create factory method for '" + item.ToString() + "'");
            return null; // Will not be reached, because Ensure.Fail throws an exception
        }

        #region Item Helper class

        /// <summary>
        /// Just a helper class
        /// </summary>
        private class Item
        {
            /// <summary>
            /// Gets or sets the filter
            /// </summary>
            public Func<ITreeViewItem, bool> Filter
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the detail view type, if we have a simple construction by InstanceCreator/ActivationContext
            /// </summary>
            public Type DetailViewType
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the factory method, which creates the detail view. 
            /// </summary>
            public Func<IActivates, DetailView> Factory
            {
                get;
                set;
            }
        }

        #endregion
    }
}

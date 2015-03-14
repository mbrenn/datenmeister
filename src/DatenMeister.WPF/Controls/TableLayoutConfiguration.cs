﻿using BurnSystems.Test;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Logic.MethodProvider;
using DatenMeister.Pool;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls
{
    /// <summary>
    /// Defines the configuration for the table view
    /// </summary>
    public class TableLayoutConfiguration : ListLayoutConfiguration
    {
        /// <summary>
        /// Gets or sets the view information, that will be used for the detail forms, after
        /// the user has selected one of the item and the item is opened.
        /// </summary>
        public IObject ViewInfoForDetailView
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the visibility of the cancel button
        /// </summary>
        public bool ShowCancelButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the visibility of the cancel button
        /// </summary>
        public bool ShowOKButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value whether the table control shall be used
        /// as a selection control. 
        /// If true, the buttons for modification will be removed, but the 
        /// OK-button will be added
        /// </summary>
        public bool UseAsSelectionControl
        {
            get;
            set;
        }

        /// <summary>
        /// This action is called, when the context menu shall be updated
        /// </summary>
        public Action<ContextMenu, IObject> UpdateContextMenu
        {
            get;
            set;
        }

        /// <summary>
        /// This event is throw, when the content is updated
        /// </summary>
        public event EventHandler ItemUpdated;

        /// <summary>
        /// Calls the <c>ItemUpdated</c> event
        /// </summary>
        internal void OnItemUpdated()
        {
            var ev = this.ItemUpdated;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the tableview object as an instance of the TableView class to 
        /// have type-safe access to the instance
        /// </summary>
        public TableView GetTableViewInfoAsTableView()
        {
            if (this.LayoutInfo == null)
            {
                return null;
            }
            else
            {
                return new TableView(this.LayoutInfo);
            }
        }

        public TableLayoutConfiguration()
        {
            this.ShowOKButton = true;

            this.UpdateContextMenu = (contextMenu, value) =>
                {
                    var methodProvider = Injection.Application.TryGet<IMethodProvider>();
                    if (methodProvider != null)
                    {
                        var method = methodProvider
                            .GetMethodOfInstanceByName(this.LayoutInfo, Entities.FieldInfos.TableView.UpdateContextMenu);
                        if (method != null)
                        {
                            method.Invoke(null, contextMenu, value);
                        }
                    }
                };
        }

        /// <summary>
        /// Sets the reflective collection by an extent. 
        /// For this option, just the elements of the extent are returned
        /// </summary>
        /// <param name="extent">Extent to be queried</param>
        public void SetByExtent(IURIExtent extent)
        {
            Ensure.That(extent != null);
            this.ElementsFactory = (x) => extent.Elements();
        }

        public void SetElements(IReflectiveCollection collection)
        {
            Ensure.That(collection != null);
            this.ElementsFactory = (x) => collection;
        }
    }
}

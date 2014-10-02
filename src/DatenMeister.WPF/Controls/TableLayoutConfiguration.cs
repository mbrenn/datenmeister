using BurnSystems.Test;
using DatenMeister.Entities.AsObject.FieldInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.WPF.Controls
{
    /// <summary>
    /// Defines the configuration for the table view
    /// </summary>
    public class TableLayoutConfiguration
    {
        /// <summary>
        /// Gets or sets the datenmeister settings of the application
        /// </summary>
        public IPublicDatenMeisterSettings Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the table view information
        /// </summary>
        public IObject TableViewInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the factory being used for the reflective collections
        /// </summary>
        public Func<IPool, IReflectiveCollection> ElementsFactory
        {
            get;
            set;
        }

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
        /// Gets the tableview object as an instance of the TableView class to have type-safe access to the instance
        /// </summary>
        public TableView TableViewInfoAsTableView
        {
            get
            {
                if (this.TableViewInfo == null)
                {
                    return null;
                }
                else
                {
                    return new TableView(this.TableViewInfo);
                }
            }
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

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
    public class FormLayoutConfiguration
    {
        /// <summary>
        /// Gets or sets the table view information
        /// </summary>
        public IObject FormViewInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the extent, where this item shall be added to
        /// Relevant, if DetailObject == null and dialog has been opened to create a new 
        /// item. 
        /// </summary>
        public IReflectiveCollection StorageCollection
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the object 
        /// </summary>
        public IObject DetailObject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type to be created, when user likes to create a new type. 
        /// May also be null, if there is no specific type. Untyped instances are also supported. 
        /// </summary>
        public IObject TypeToCreate
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the display mode
        /// </summary>
        public EditMode EditMode
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the display mode
        /// </summary>
        public DisplayMode DisplayMode
        {
            get { return Controls.DisplayMode.Form; }
        }

        /// <summary>
        /// Gets the tableview object as an instance of the TableView class to have type-safe access to the instance
        /// </summary>
        public FormView GetFormViewInfoAsFormView()
        {
            if (this.FormViewInfo == null)
            {
                return null;
            }
            else
            {
                return new FormView(this.FormViewInfo);
            }
        }
    }
}

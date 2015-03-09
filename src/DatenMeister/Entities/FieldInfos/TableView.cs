using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class TableView : View
    {
        public const string UpdateContextMenu = "UpdateContextMenu";

        /// <summary>
        /// Initializes a new instance of the TableView class.
        /// </summary>
        public TableView()
        {
            this.allowEdit = true;
            this.allowNew = true;
            this.allowDelete = true;
        }

        /// <summary>
        /// Gets or sets the uri that shall be shown
        /// </summary>
        public string extentUri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maintype that is used to open dialogs, where user can create
        /// a new object out of the table view. 
        /// </summary>
        public IObject mainType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a list of maintypes that are used to open dialogs, where user can create
        /// a new object out of the table view. 
        /// </summary>
        public IList<object> typesForCreation
        {
            get;
            set;
        }

        [DefaultValue(true)]
        public bool allowEdit
        {
            get;
            set;
        }

        [DefaultValue(true)]
        public bool allowDelete
        {
            get;
            set;
        }

        [DefaultValue(true)]
        public bool allowNew
        {
            get;
            set;
        }
    }
}

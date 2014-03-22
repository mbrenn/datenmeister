using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class TableView : View
    {
        /// <summary>
        /// Initializes a new instance of the TableView class.
        /// </summary>
        public TableView()
        {
            this.allowEdit = true;
            this.allowNew = true;
            this.allowDelete = true;
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

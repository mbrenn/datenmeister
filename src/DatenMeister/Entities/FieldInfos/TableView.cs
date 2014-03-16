using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class TableView : View
    {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class View : General
    {
        public List<object> fieldInfos
        {
            get;
            set;
        }

        public bool allowEdit
        {
            get;
            set;
        }

        public bool allowDelete
        {
            get;
            set;
        }

        public bool allowNew
        {
            get;
            set;
        }

        public bool startInEditMode
        {
            get;
            set;
        }
    }
}

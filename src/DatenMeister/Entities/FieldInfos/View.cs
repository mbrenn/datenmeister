using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public bool startInEditMode
        {
            get;
            set;
        }
    }
}

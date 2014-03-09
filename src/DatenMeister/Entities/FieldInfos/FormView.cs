using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class FormView : View
    {
        public bool showColumnHeaders
        {
            get;
            set;
        }

        public bool allowNewProperty
        {
            get;
            set;
        }
    }
}

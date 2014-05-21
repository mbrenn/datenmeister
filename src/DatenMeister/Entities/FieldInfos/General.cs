using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    /// <summary>
    /// Stores the general properties for all field informations
    /// </summary>
    public class General
    {
        public string name
        {
            get;
            set;
        }

        public string binding
        {
            get;
            set;
        }

        public bool isReadOnly
        {
            get;
            set;
        }
    }

}

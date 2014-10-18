using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Entities.FieldInfos
{
    /// <summary>
    /// Defines the field as a hyperlink column. 
    /// Only makes sense for table views
    /// </summary>
    public class HyperLinkColumn : TextField
    {
        public HyperLinkColumn()
        {
        }

        public HyperLinkColumn(string name, string binding)
            : base(name, binding)
        {
        }
    }
}

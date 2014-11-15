using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Controls
{
    public class ObjectForTreeView
    {
        public ObjectForTreeView(IObject value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the value that shall be represented
        /// </summary>
        public IObject Value
        {
            get;
            set;
        }

        /// <summary>
        /// Overrides to a string
        /// </summary>
        /// <returns>The string representation of the item</returns>
        public override string ToString()
        {
            this.Value.ToString();
            return base.ToString();
        }
    }
}

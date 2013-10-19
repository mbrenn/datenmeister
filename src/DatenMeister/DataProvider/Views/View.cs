using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Views
{
    public class View
    {
        public string name
        {
            get;
            set;
        }

        public View()
        {
            this.fieldInfos = new List<FieldInfo>();
        }

        public List<FieldInfo> fieldInfos
        {
            get;
            set;
        }
    }
}

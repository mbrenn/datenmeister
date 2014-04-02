using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class Checkbox : General
    {
        public Checkbox(string title, string name)
        {
            this.title = title;
            this.name = name;
        }
    }
}

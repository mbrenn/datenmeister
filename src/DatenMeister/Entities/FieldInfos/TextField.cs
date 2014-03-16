using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class TextField : General
    {
        public TextField(string title, string name)
        {
            this.title = title;
            this.name = name;
        }

        public int width
        {
            get;
            set;
        }

        public int height
        {
            get;
            set;
        }
    }
}

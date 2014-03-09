using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class Comment : General
    {
        public Comment(string title, string comment)
        {
            this.title = title;
            this.comment = comment;
        }

        public string comment
        {
            get;
            set;
        }
    }
}

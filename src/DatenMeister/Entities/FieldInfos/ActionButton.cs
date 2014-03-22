using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class ActionButton
    {
        public ActionButton(string text, string clickUrl)
        {
            this.text = text;
            this.clickUrl = clickUrl;
        }

        public string text
        {
            get;
            set;
        }

        public string clickUrl
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatenMeister.AddOns.Scripting.ScriptObjects
{
    public class Message
    {
        public void ToUser(string message)
        {
            MessageBox.Show(message);
        }

    }
}

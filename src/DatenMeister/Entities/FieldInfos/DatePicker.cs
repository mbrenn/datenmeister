using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Entities.FieldInfos
{
    /// <summary>
    /// Shows a date time picker
    /// </summary>
    public class DatePicker : General
    {
        public DatePicker()
        {
        }

        public DatePicker(string name, string binding)
            : base(name, binding)
        {
        }
    }
}

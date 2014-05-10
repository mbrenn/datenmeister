using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic
{
    public class ObjectDictionaryForView : ObjectDictionary
    {

        /// <summary>
        /// Initializes a new instance of the ObjectDictionary class
        /// </summary>
        /// <param name="value">Value to be set</param>
        public ObjectDictionaryForView(IObject value)
            : base(value)
        {
        }
    }
}

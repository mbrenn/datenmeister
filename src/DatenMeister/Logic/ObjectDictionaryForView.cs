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

        public override object Get(string index)
        {
            var result = base.Get(index).AsSingle();
            var resultAsIObject = result as IObject;
            if (resultAsIObject != null)
            {
                return resultAsIObject.get("name").AsSingle().ToString();
            }

            if (result != null)
            {
                return result.ToString();
            }

            return "NULL";
        }
    }
}


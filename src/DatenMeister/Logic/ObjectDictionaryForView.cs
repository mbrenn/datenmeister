using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

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
            if (this.Value.isSet(index))
            {
                var result = base.Get(index).AsSingle();
                var resultAsIObject = result as IObject;

                if (resultAsIObject != null)
                {
                    // Here, we have the redirection for referenced objcts
                    return resultAsIObject.get("name").AsSingle().ToString();
                }

                result = result.AsSingle();
                if (result != null)
                {
                    if (result is DateTime)
                    {
                        return ((DateTime)result).ToString(Thread.CurrentThread.CurrentCulture);
                    }

                    // If the object is not an IObject, we just use ToString
                    return result.ToString();
                }
            }

            return "NULL";
        }
    }
}


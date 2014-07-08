using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Entities.AsObject.Uml;
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
        /// Stores the field information
        /// </summary>
        private IEnumerable<IObject> fieldInfos;

        /// <summary>
        /// Initializes a new instance of the ObjectDictionary class
        /// </summary>
        /// <param name="value">Value to be set</param>
        public ObjectDictionaryForView(IObject value, IEnumerable<IObject> fieldInfos = null)
            : base(value)
        {
            this.fieldInfos = fieldInfos;
        }

        public override object Get(string index)
        {
            if (this.Value.isSet(index))
            {
                var fieldInfo = this.FindFieldInfo(index);

                var result = GetUnmanipulatedContent(index);
                if (fieldInfo != null)
                {
                    if (TextField.isDateTime(fieldInfo))
                    {
                        DateTime dt;
                        if (DateTime.TryParse(
                            result,
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.AssumeLocal,
                            out dt))
                        {
                            return dt.ToString(Thread.CurrentThread.CurrentCulture);
                        }
                    }
                }

                return result;
            }

            return "NULL";
        }

        /// <summary>
        /// Gets the unmanipulated content of the object
        /// </summary>
        /// <param name="index">Index to be queried</param>
        /// <returns>Content to be shown. This content might be changed by field information</returns>
        private string GetUnmanipulatedContent(string index)
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
            else
            {
                return "NULL";
            }
        }

        /// <summary>
        /// Finds the fieldinformation by an index
        /// </summary>
        /// <param name="index">Index to be queried</param>
        /// <returns>Found object or null, if not found</returns>
        private IObject FindFieldInfo(string index)
        {
            if (this.fieldInfos == null)
            {
                return null;
            }

            foreach (var fieldInfo in this.fieldInfos)
            {
                if (General.getBinding(fieldInfo).AsSingle().ToString() == index)
                {
                    return fieldInfo;
                }
            }

            return null;
        }
    }
}


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
        /// Got the Binding name to find a type
        /// </summary>
        public const string TypeBinding = "4DCDDF6D-B734-467D-81D2-F3E3CE4DBA39";

        /// <summary>
        /// Got the Binding name to find the extent url
        /// </summary>
        public const string ExtentUriBinding = "45FAC4BA-EF5A-4182-A4A2-FED3BF430B7D";

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
            var fieldInfo = this.FindFieldInfo(index);

            var result = this.GetUnmanipulatedContent(index);

            if (result != null)
            {
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

                    return result;
                }
                else
                {
                    return result;
                }
            }

            return ObjectHelper.Null;
        }

        /// <summary>
        /// Gets the unmanipulated content of the object
        /// </summary>
        /// <param name="index">Index to be queried</param>
        /// <returns>Content to be shown. This content might be changed by field information</returns>
        private string GetUnmanipulatedContent(string index)
        {
            object result = "NULL";

            // Gets the index and its result
            result = GetContentByBinding(index);
            var resultAsIObject = result as IObject;

            if (resultAsIObject != null)
            {
                // We have an IObject which has been referenced. 
                // We return name of the object
                result = resultAsIObject.get("name").AsSingle().ToString();
            }

            result = result.AsSingle();
            if (result != null)
            {
                if (result is DateTime)
                {
                    result = ((DateTime)result).ToString(Thread.CurrentThread.CurrentCulture);
                }
                else
                {
                    // If the object is not an IObject, we just use ToString
                    result = result.ToString();
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Gets the property content 
        /// </summary>
        /// <param name="bindingName">Name of the binding to be used</param>
        /// <returns>Returned object</returns>
        private object GetContentByBinding(string bindingName)
        {
            object result;
            if (bindingName == TypeBinding)
            {
                var element = base.Value as IElement;
                if (element != null)
                {
                    result = element.getMetaClass();
                }
                else
                {
                    result = ObjectHelper.NotSet;
                }
            }
            else if (bindingName == ExtentUriBinding)
            {
                var extent = base.Value.Extent;
                if (extent != null)
                {
                    result = extent.ContextURI();
                }
                else
                {
                    result = ObjectHelper.NotSet;
                }
            }
            else
            {
                if (this.Value.isSet(bindingName))
                {
                    result = base.Get(bindingName).AsSingle();
                }
                else
                {
                    result = ObjectHelper.NotSet;
                }
            }

            return result;
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


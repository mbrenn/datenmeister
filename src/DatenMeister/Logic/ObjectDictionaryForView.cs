using BurnSystems;
using DatenMeister.DataProvider;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Entities.AsObject.Uml;
using System;
using System.Collections;
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

        /// <summary>
        /// Checks, if the given property is set. 
        /// If the object is not set, false will be returned
        /// </summary>
        /// <param name="propertyName">Index to be set</param>
        /// <returns>true, property is set</returns>
        public bool IsSet ( string propertyName)
        {
            if (this.Value.get(propertyName).AsSingle() == ObjectHelper.NotSet)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override object Get(string index)
        {
            var fieldInfo = this.FindFieldInfo(index);

            var result = this.GetInvariantStringRepresentation(index);

            if (result != null)
            {
                if (fieldInfo != null)
                {
                    // If the field shall be used as DateTime
                    // If yes, read it as invariant culture and convert it to local culture
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
        private string GetInvariantStringRepresentation(string index)
        {
            object result = "NULL";

            // Gets the index and its result
            result = GetContentByBinding(index);
            return this.GetInvariantStringRepresentation(index, result);
        }

        /// <summary>
        /// Gets the unmanipulated context
        /// </summary>
        /// <param name="index">Name of the property which contained the string</param>
        /// <param name="result">Value of the result to be converted</param>
        /// <returns>Result als string</returns>
        private string GetInvariantStringRepresentation(string index, object result)
        {
            // Check for IUnspecific... 
            var resultAsUnspecified = result as IUnspecified;
            if (resultAsUnspecified != null)
            {
                if (resultAsUnspecified.PropertyValueType == PropertyValueType.Enumeration)
                {
                    result = resultAsUnspecified.AsEnumeration();
                }

                if (resultAsUnspecified.PropertyValueType == PropertyValueType.Single)
                {
                    result = resultAsUnspecified.AsSingle();
                }
            }

            var resultAsIObject = result as IObject;

            if (resultAsIObject != null)
            {
                // We have an IObject which has been referenced. 
                // We return name of the object
                result = resultAsIObject.get("name").AsSingle().ToString();
            }

            // Checks, if this is an enumeration, if yes, do enumerate and collect 
            if (result is IEnumerable && !(result is string))
            {
                var builder = new StringBuilder();
                var notFirst = false;

                foreach (var item in result as IEnumerable)
                {
                    if (notFirst)
                    {
                        builder.Append("\r\n");
                    }

                    builder.Append(this.GetInvariantStringRepresentation(index, item));
                    notFirst = true;
                }

                result = builder.ToString();
            }
            else
            {
                // Assume that this is a single
                result = result.AsSingle();
                if (result != null)
                {
                    result = ConvertToStringForView(result);
                }
            }

            return result.ToString();
        }

        private static object ConvertToStringForView(object result)
        {
            if (result is DateTime)
            {
                result = ((DateTime)result).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                // If the object is not an IObject, we just use ToString
                result = result.ToString();
            }

            return result;
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
                result = this.GetTypeBindingContent();
            }
            else if (bindingName == ExtentUriBinding)
            {
                result = this.GetExtentURIContent();
            }
            else
            {
                if (this.Value.isSet(bindingName))
                {
                    result = base.Get(bindingName);
                }
                else
                {
                    result = ObjectHelper.NotSet;
                }
            }

            return result;
        }

        private object GetTypeBindingContent()
        {
            object result;
            var element = base.Value as IElement;
            if (element != null)
            {
                result = element.getMetaClass();
            }
            else
            {
                result = ObjectHelper.NotSet;
            }
            return result;
        }

        /// <summary>
        /// Gets the content of the extent URI being associated to the object
        /// </summary>
        /// <returns></returns>
        private object GetExtentURIContent()
        {
            object result;
            var extent = base.Value.Extent;
            if (extent != null)
            {
                result = extent.ContextURI();
            }
            else
            {
                result = ObjectHelper.NotSet;
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

        /// <summary>
        /// Gets all values as a key value pair
        /// </summary>
        /// <returns>Enumeration of key value pairs</returns>
        public IEnumerable<KeyValuePair<string, object>> GetAllVisible()
        {
            foreach (var fieldInfo in this.fieldInfos)
            {
                var name = General.getBinding(fieldInfo).AsSingle().ToString();
                yield return new KeyValuePair<string, object>(name, this.Get(name));
            }
        }

        /// <summary>
        /// Checks by the
        /// </summary>
        /// <param name="value"></param>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool FilterByText(ObjectDictionaryForView value, string filterByText)
        {
            filterByText = filterByText.ToLower();

            if (value.GetAllVisible().Any(x => x.ToString().ToLower().Contains(filterByText)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsSpecialBinding(string bindingInfo)
        {
            if (bindingInfo == ObjectDictionaryForView.ExtentUriBinding
                || bindingInfo == ObjectDictionaryForView.TypeBinding)
            {
                return true;
            }

            return false;
        }
    }
}


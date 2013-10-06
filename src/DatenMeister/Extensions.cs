using DatenMeister.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    public static class Extensions
    {
        public static JsonExtentInfo ToJson(this IURIExtent extent)
        {
            return new JsonExtentInfo()
            {
                uri = extent.ContextURI(),
                type = extent.GetType().FullName
            };
        }

        /// <summary>
        /// Transforms the given object to a pure Json-Object that can be used for web interaction
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Converted object</returns>
        public static Dictionary<string, object> ToFlatObject(this IObject value)
        {
            var result = new Dictionary<string, object>();

            foreach (var pair in value.GetAll())
            {
                var pairValue = ConvertAsFlatObject(pair.Value);

                result[pair.PropertyName] = pairValue;
            }

            return result;
        }

        private static object ConvertAsFlatObject(object pairValue)
        {
            if (IsNative(pairValue))
            {
                return pairValue;
            }

            if (pairValue is IEnumerable)
            {
                var listElements = new List<object>();
                foreach (var enumerationValue in pairValue as IEnumerable)
                {
                    listElements.Add(ConvertAsFlatObject(enumerationValue));
                }

                return listElements;
            }
            
            if (pairValue is IObject)
            {
                return ToFlatObject(pairValue as IObject);
            }

            return pairValue;
        }

        public static void SetAll(this IObject value, Dictionary<string, object> values)
        {
            foreach (var pair in values)
            {
                value.Set(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Stores a list of all primitive types that are supported by DatenMeister directly
        /// </summary>
        private static List<Type> primitiveTypes = new List<Type>();

        /// <summary>
        /// Checks, if the given object is a native object or if the objects needs to be converted
        /// </summary>
        /// <param name="checkObject">CheckObject to be checked</param>
        /// <returns>true, if object is not native</returns>
        public static bool IsNative(object checkObject)
        {
            if (primitiveTypes.Count == 0)
            {
                lock (primitiveTypes)
                {
                    primitiveTypes.Clear();
                    primitiveTypes.Add(typeof(Int16));
                    primitiveTypes.Add(typeof(Int32));
                    primitiveTypes.Add(typeof(Int64));
                    primitiveTypes.Add(typeof(Double));
                    primitiveTypes.Add(typeof(Single));
                    primitiveTypes.Add(typeof(String));
                    primitiveTypes.Add(typeof(DateTime));
                }
            }

            return primitiveTypes.Contains(checkObject.GetType());
        }
    }
}

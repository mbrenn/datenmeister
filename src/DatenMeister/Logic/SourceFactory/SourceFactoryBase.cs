using BurnSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.SourceFactory
{
    /// <summary>
    /// Defines some basic helper methods being used for source factories
    /// </summary>
    public class SourceFactoryBase
    {
        /// <summary>
        /// Contains four spaces
        /// </summary>
        internal static string FourSpaces = "    ";
        internal static string EightSpaces = FourSpaces + FourSpaces;
        internal static string TwelveSpaces = FourSpaces + EightSpaces;
        internal static string SixteenSpaces = FourSpaces + TwelveSpaces;
        internal static string TwentySpaces = FourSpaces + SixteenSpaces;

        /// <summary>
        /// Stores the provider
        /// </summary>
        protected ITypeInfoProvider provider;

        /// <summary>
        /// Initializes a new instance of the TypeScriptSource Factory
        /// </summary>
        /// <param name="provider">Provider to be used</param>
        public SourceFactoryBase(ITypeInfoProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Returns the method name for the get method
        /// </summary>
        /// <param name="propertyName">Name of the property to be used</param>
        /// <param name="propertyType">Type being used</param>
        /// <returns>The GetMethod</returns>
        public string GetGetMethodName( string propertyName, Type propertyType)
        {
            var functionName = string.Empty;
            if (propertyName.ToLower().StartsWith("is") && propertyType == typeof(Boolean))
            {
                // Some heuristics:
                // Boolean properties, starting with 'is' will have a get method without modification
                // bool isReadOnly -> bool isReadOnly
                functionName = propertyName;
            }
            else
            {
                functionName = "get" + StringManipulation.ToUpperFirstLetter(propertyName);
            }

            return functionName;
        }

        /// <summary>
        /// Returns the method name for the set method
        /// </summary>
        /// <param name="propertyName">Name of the property to be used</param>
        /// <param name="propertyType">Type being used</param>
        /// <returns>The setMethod</returns>
        public string GetSetMethodName(string propertyName, Type propertyType)
        {
            var functionName = string.Empty;
            if (propertyName.ToLower().StartsWith("is") && propertyType == typeof(Boolean))
            {
                // Some heuristics:
                // Boolean properties, starting with 'is' will have a set method without 'is'
                // bool isReadOnly -> bool setReadOnly
                functionName = "set" + StringManipulation.ToUpperFirstLetter(propertyName.Substring(2));
            }
            else
            {
                functionName = "set" + StringManipulation.ToUpperFirstLetter(propertyName);
            }

            return functionName;
        }

        /// <summary>
        /// Checks, if the property Type should have a push method
        /// </summary>
        /// <param name="propertyType">Type to be checked</param>
        /// <returns>true, if property type has a push method</returns>
        public bool HasPushMethod(Type propertyType)
        {
            if (typeof(IEnumerable).IsAssignableFrom(propertyType) && propertyType != typeof(string))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the method name for the push method
        /// </summary>
        /// <param name="propertyName">Name of the property to be used</param>
        /// <param name="propertyType">Type being used</param>
        /// <returns>The push Method</returns>
        public string GetPushMethodName(string propertyName, Type propertyType)
        {
            var functionName = propertyName;
            // Check, if propertyname has plural 's' at end
            if (propertyName.EndsWith("s"))
            {
                functionName = propertyName.Substring(0, propertyName.Length - 1);
            }

            return "push" + StringManipulation.ToUpperFirstLetter(functionName);
        }
    }
}

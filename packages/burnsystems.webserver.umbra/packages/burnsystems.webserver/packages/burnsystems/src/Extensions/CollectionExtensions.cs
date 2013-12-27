//-----------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Extensions
{
    using System.Collections.Generic;

    /// <summary>
    /// These collection extensions are used to ease the use
    /// collector class
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Sets a key-value pair in the dictionary
        /// </summary>
        /// <typeparam name="T">Type of the key</typeparam>
        /// <typeparam name="TValue">Type of the value</typeparam>
        /// <param name="dict">Dictionary to be modified</param>
        /// <param name="key">Key to be set</param>
        /// <param name="value">Value, which shall be assigned to key</param>
        /// <returns>The same dictionary. </returns>
        public static Dictionary<T, TValue> With<T, TValue>(
            this Dictionary<T, TValue> dict,
            T key,
            TValue value)
        {
            dict[key] = value;
            return dict;
        }

        /// <summary>
        /// Sets an enumeration of key value pairs into the
        /// dictionary
        /// </summary>
        /// <typeparam name="T">Type of the key</typeparam>
        /// <typeparam name="TValue">Type of the value</typeparam>
        /// <param name="dict">Dictionary to be updated</param>
        /// <param name="values">Values to be set</param>
        /// <returns>The modified dictionary</returns>
        public static Dictionary<T, TValue> With<T, TValue>(
            this Dictionary<T, TValue> dict,
            IEnumerable<KeyValuePair<T, TValue>> values)
        {
            foreach (var pair in values)
            {
                dict[pair.Key] = pair.Value;
            }

            return dict;
        }
    }
}

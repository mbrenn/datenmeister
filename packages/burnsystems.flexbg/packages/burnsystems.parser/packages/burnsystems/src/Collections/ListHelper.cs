//-----------------------------------------------------------------------
// <copyright file="ListHelper.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using BurnSystems.Test;

    /// <summary>
    /// Defines the delegate for the procedure
    /// </summary>
    [Obsolete("Use Action")]
    public delegate void Procedure();

    /// <summary>
    /// Defines the delegate for the procedure
    /// </summary>    
    /// <typeparam name="T">Type of first argument</typeparam>
    /// <param name="arg">First argument</param>
    [Obsolete("Use Action")]
    public delegate void Procedure<T>(T arg);

    /// <summary>
    /// Defines the delegate for the procedure
    /// </summary>    
    /// <typeparam name="T1">Type of first argument</typeparam>
    /// <typeparam name="T2">Type of second argument</typeparam>
    /// <param name="arg1">First argument</param>
    /// <param name="arg2">Second argument</param>
    [Obsolete("Use Action")]
    public delegate void Procedure<T1, T2>(T1 arg1, T2 arg2);

    /// <summary>
    /// Defines the delegate for the procedure
    /// </summary>    
    /// <typeparam name="T1">Type of first argument</typeparam>
    /// <typeparam name="T2">Type of second argument</typeparam>
    /// <typeparam name="T3">Type of third argument</typeparam>
    /// <param name="arg1">First argument</param>
    /// <param name="arg2">Second argument</param>
    /// <param name="arg3">Third argument</param>
    [Obsolete("Use Action")]
    public delegate void Procedure<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

    /// <summary>
    /// Backports delegate <c>Action</c> of System.Core
    /// </summary>
    [Obsolete("Use Func")]
    public delegate void Function();

    /// <summary>
    /// Backports delegate <c>Func</c> of System.Core
    /// </summary>
    /// <typeparam name="TResult">Type of result</typeparam>
    /// <returns>Result of function</returns>
    [Obsolete("Use Func")]
    public delegate TResult Function<TResult>();

    /// <summary>
    /// Backports delegate <c>Func</c> of System.Core
    /// </summary>
    /// <typeparam name="TResult">Type of result</typeparam>
    /// <typeparam name="T">Type of first parameter</typeparam>
    /// <param name="parameter">Parameter of function</param>
    /// <returns>Result of function</returns>
    [Obsolete("Use Func")]
    public delegate TResult Function<TResult, T>(T parameter);

    /// <summary>
    /// Backports delegate <c>Func</c> of System.Core
    /// </summary>
    /// <param name="o1">First parameter</param>
    /// <param name="o2">Second parameter</param>
    /// <typeparam name="TResult">Type of result</typeparam>
    /// <typeparam name="T1">Type of first parameter</typeparam>
    /// <typeparam name="T2">Type of second parameter</typeparam>    
    /// <returns>Result of function</returns>
    [Obsolete("Use Func")]
    public delegate TResult Function<TResult, T1, T2>(T1 o1, T2 o2);

    /// <summary>
    /// Backports delegate <c>Func</c> of System.Core
    /// </summary>
    /// <param name="o1">First parameter</param>
    /// <param name="o2">Second parameter</param>
    /// <param name="o3">Third parameter</param>
    /// <typeparam name="TResult">Type of result</typeparam>
    /// <typeparam name="T1">Type of first parameter</typeparam>
    /// <typeparam name="T2">Type of second parameter</typeparam>    
    /// <typeparam name="T3">Type of third parameter</typeparam>
    /// <returns>Result of function</returns>
    [Obsolete("Use Func")]
    public delegate TResult Function<TResult, T1, T2, T3>(T1 o1, T2 o2, T3 o3);

    /// <summary>
    /// Diese Listenhelferklasse stellt ein paar Funktionen für die
    /// unterschiedlichen Listenklassen zur Verfügung. 
    /// </summary>
    public static class ListHelper
    {
        /// <summary>
        /// Es wird jedes Element in der Aufzählung durchgegangen und das Delegat
        /// mit diesem Element aufgerufen
        /// </summary>
        /// <typeparam name="T">Typ des Delegaten</typeparam>
        /// <param name="list">Aufzählung mit den Elementen</param>
        /// <param name="action">Delegat, der aufgerufen wird</param>
        public static void ForEach<T>(
            IEnumerable<T> list, 
            Action<T> action)
        {
            Ensure.IsNotNull(list);

            foreach (T element in list)
            {
                action(element);
            }
        }

        /// <summary>
        /// Überprüft, ob auf ein bestimmtes Element in der Liste das 
        /// Prädikat zutrifft
        /// </summary>
        /// <typeparam name="T">Typ der Elemente in der Liste</typeparam>
        /// <param name="list">List to be checked</param>
        /// <param name="predicate">Prädikat, auf das jedes einzelne
        /// Element getestet wird. </param>
        /// <returns>true, wenn eines der Elemente zutrifft</returns>
        public static bool Exists<T>(IEnumerable<T> list, Predicate<T> predicate)
        {
            Ensure.IsNotNull(list);

            foreach (var element in list)
            {
                if (predicate(element))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gibt das erste Element zurück, dass das Prädikat erfüllt
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="list">List to be checked</param>
        /// <param name="predicate">Zu erfüllendes Prädikat</param>
        /// <returns>Gefundenes Objekt</returns>
        public static T Find<T>(IEnumerable<T> list, Predicate<T> predicate)
        {
            Ensure.IsNotNull(list);

            foreach (T element in list)
            {
                if (predicate(element))
                {
                    return element;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Checks, whether the item is contained in the given list
        /// </summary>
        /// <typeparam name="T">Type of the queried item</typeparam>
        /// <param name="list">List to be looked up</param>
        /// <param name="item">Item to be checked</param>
        /// <returns>true, if item is found in list</returns>
        public static bool Contains<T>(IEnumerable<T> list, T item)
        {
            Ensure.IsNotNull(list);

            foreach (var currentItem in list)
            {
                if (currentItem.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks, whether the given predicate becomes true in the list
        /// </summary>
        /// <typeparam name="T">Type of the queried item</typeparam>
        /// <param name="list">List to be looked up</param>
        /// <param name="predicate">Predicate to be used</param>
        /// <returns>true, if item is found in list</returns>
        public static bool Contains<T>(IEnumerable<T> list, Predicate<T> predicate)
        {
            Ensure.IsNotNull(list);

            foreach (var currentItem in list)
            {
                if (predicate(currentItem))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Converts an enumeration to an array
        /// </summary>
        /// <typeparam name="T">Type of element in enumeration</typeparam>
        /// <param name="list">List with elements</param>
        /// <returns>Array with elements</returns>
        [Obsolete("System.Linq.Enumerable.ToArray()")]
        public static T[] ToArray<T>(IEnumerable<T> list)
        {
            Ensure.IsNotNull(list);

            var listTemp = new List<T>(list);
            return listTemp.ToArray();
        }

        /// <summary>
        /// Converts an enumeration to an array
        /// </summary>
        /// <typeparam name="T">Type of element in enumeration</typeparam>
        /// <param name="list">List with elements</param>
        /// <returns>Array with elements</returns>
        [Obsolete("System.Linq.Enumerable.ToArray()")]
        public static T[] ToArray<T>(ICollection<T> list)
        {
            Ensure.IsNotNull(list);

            var listTemp = new T[list.Count];
            var n = 0; 
            foreach (var item in list)
            {
                listTemp[n] = item;
                n++;
            }

            return listTemp;
        }

        /// <summary>
        /// Returns all elements matching the predicate
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="list">List to be evaluated</param>
        /// <param name="predicate">Predicate for items</param>
        /// <returns>The elements matching the predicate</returns>
        [Obsolete("System.Linq.Enumerable.Where()")]
        public static IEnumerable<T> Where<T>(IEnumerable<T> list, Predicate<T> predicate)
        {
            Ensure.IsNotNull(list);

            foreach (var item in list)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Gibt eine Aufzählung aller Inhalte zurück, die das übergebene
        /// Prädikat erfüllen
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="list">Liste mit den Elementen, die zu überprüfen sind</param>
        /// <param name="predicate">Gefordertes Prädikat</param>
        /// <returns>Aufzählung mit passenden Elementen</returns>
        [Obsolete("System.Linq.Enumerable.Where()")]
        public static IEnumerable<T> FindAll<T>(
            IEnumerable<T> list, 
            Predicate<T> predicate)
        {
            Ensure.IsNotNull(list);

            foreach (var element in list)
            {
                if (predicate(element))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Konvertiert ein aufzählbares Objekt eines bestimmten Types
        /// mit Hilfe eines Delegaten in einen anderen Typ.
        /// </summary>
        /// <typeparam name="TSource">Typ der Quellobjekte</typeparam>
        /// <typeparam name="TResult">Typ der Zielobjekt</typeparam>
        /// <param name="source">Array mit den Quellelementen</param>
        /// <param name="converter">Konverter mit dem Resultat</param>
        /// <returns>Liste mit dem konvertierten Elementen</returns>
        [Obsolete("System.Linq.Enumerable.Select()")]
        public static IList<TResult> Convert<TSource, TResult>(
            IEnumerable<TSource> source, 
            Converter<TSource, TResult> converter)
        {
            Ensure.IsNotNull(source);

            var result = new List<TResult>();

            foreach (var element in source)
            {
                result.Add(converter(element));
            }

            return result;
        }

        /// <summary>
        /// Konvertiert ein aufzählbares Objekt eines bestimmten Types
        /// mit Hilfe eines Delegaten in einen anderen Typ.
        /// </summary>
        /// <typeparam name="TSource">Typ der Quellobjekte</typeparam>
        /// <typeparam name="TResult">Typ der Zielobjekt</typeparam>
        /// <param name="source">Array mit den Quellelementen</param>
        /// <param name="converter">Konverter mit dem Resultat</param>
        /// <returns>Array mit dem konvertierten Elementen</returns>
        [Obsolete("System.Linq.Enumerable.Select().ToArray()")]
        public static TResult[] ConvertToArray<TSource, TResult>(
            IEnumerable<TSource> source, 
            Converter<TSource, TResult> converter)
        {
            Ensure.IsNotNull(source);

            var result = new List<TResult>();

            foreach (var element in source)
            {
                result.Add(converter(element));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Identifies Duplicates in list and only returns unique elements
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="source">List with elements</param>
        /// <returns>Enumeration of unique elements</returns>
        public static IEnumerable<T> Distinct<T>(
            this IEnumerable<T> source)
        {
            Ensure.IsNotNull(source);

            var found = new List<T>();

            foreach (var element in source)
            {
                if (found.IndexOf(element) == -1)
                {
                    // Noch nicht in der Liste, hinzufügen und zurückgeben
                    yield return element;
                    found.Add(element);
                }
            }
        }

        /// <summary>
        /// Identifies duplicates in list and only returns unique elements. 
        /// The Selector is used to get the unique property of the element
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <typeparam name="TValue">Type of the selected key</typeparam>
        /// <param name="source">List with elements</param>
        /// <param name="selector">Selector to find unique element</param>
        /// <returns>Enumeration of unique elements</returns>
        public static IEnumerable<T> Distinct<T, TValue>(
            this IEnumerable<T> source,
            Func<T, TValue> selector)
        {
            Ensure.IsNotNull(source);

            var found = new List<TValue>();

            foreach (var element in source)
            {
                var key = selector(element);
                if (found.IndexOf(key) == -1)
                {
                    // Noch nicht in der Liste, hinzufügen und zurückgeben
                    yield return element;
                    found.Add(key);
                }
            }
        }

        /// <summary>
        /// Gibt die Anzahl der Elemente zurück, die ein bestimmtes
        /// Prädikat erfüllen
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="source">Elemente, die zu prüfen sind</param>
        /// <param name="predicate">Das zu erfüllende Prädikat</param>
        /// <returns>Anzahl der gefundenen Elemente</returns>
        [Obsolete("System.Linq.Enumerable.Count()")]
        public static int Count<T>(IEnumerable<T> source, Predicate<T> predicate)
        {
            Ensure.IsNotNull(source);

            var result = 0;
            foreach (var element in source)
            {
                if (predicate(element))
                {
                    result++;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the first element of enumeration
        /// </summary>
        /// <typeparam name="T">Type of elements in source</typeparam>
        /// <param name="source">Source, from which the last element should be get</param>
        /// <returns>First element of list, or <c>default(T)</c> if no element exists</returns>
        [Obsolete("System.Linq.Enumerable.FirstOrDefault()")]
        public static T GetFirstElement<T>(IEnumerable<T> source)
        {
            Ensure.IsNotNull(source);

            foreach (var item in source)
            {
                return item;
            }

            return default(T);
        }

        /// <summary>
        /// Gets the first element of collection
        /// </summary>
        /// <typeparam name="T">Type of elements in source</typeparam>
        /// <param name="source">Source, from which the last element should be get</param>
        /// <returns>First element of list, or <c>default(T)</c> if no element exists</returns>
        [Obsolete("System.Linq.Enumerable.FirstOrDefault()")]
        public static T GetFirstElement<T>(IList<T> source)
        {
            Ensure.IsNotNull(source);

            if (source.Count == 0)
            {
                return default(T);
            }

            return source[0];
        }

        /// <summary>
        /// Gets the last element of enumeration
        /// </summary>
        /// <typeparam name="T">Type of elements in source</typeparam>
        /// <param name="source">Source, from which the last element should be get</param>
        /// <returns>Last element of list, or <c>default(T)</c> if no element exists</returns>
        [Obsolete("System.Linq.Enumerable.LastOrDefault()")]        
        public static T GetLastElement<T>(IEnumerable<T> source)
        {
            Ensure.IsNotNull(source);

            var result = default(T);
            foreach (var item in source)
            {
                result = item;
            }

            return result;
        }

        /// <summary>
        /// Gets the last element of collection
        /// </summary>
        /// <typeparam name="T">Type of elements in source</typeparam>
        /// <param name="source">Source, from which the last element should be get</param>
        /// <returns>Last element of list, or <c>default(T)</c> if no element exists</returns>
        [Obsolete("System.Linq.Enumerable.LastOrDefault()")]
        public static T GetLastElement<T>(IList<T> source)
        {
            Ensure.IsNotNull(source);

            if (source.Count == 0)
            {
                return default(T);
            }

            return source[source.Count - 1];
        }

        /// <summary>
        /// Gibt die Summe der konvertierten Objekte zurück
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="source">List with elements</param>
        /// <param name="converter">Delegate used for conversion of an 
        /// element to a long-value</param>
        /// <returns>Summe der Rückgabewerte</returns>
        [Obsolete("System.Linq.Enumerable.Sum()")]
        public static long Sum<T>(
            IEnumerable<T> source, 
            Converter<T, long> converter)
        {
            Ensure.IsNotNull(source);

            var result = 0L;

            foreach (var element in source)
            {
                result += converter(element);
            }

            return result;
        }

        /// <summary>
        /// Gibt die Summe der konvertierten Objekte zurück
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="source">List with elements</param>
        /// <param name="converter">Delegate used for conversion of an 
        /// element to a double-value</param>
        /// <returns>Summe der Rückgabewerte</returns>
        [Obsolete("System.Linq.Enumerable.Sum()")]
        public static double Sum<T>(
            IEnumerable<T> source, 
            Converter<T, double> converter)
        {
            Ensure.IsNotNull(source);

            var result = 0.0;

            foreach (var element in source)
            {
                result += converter(element);
            }

            return result;
        }

        /// <summary>
        /// Überprüft mit Hilfe des übergebenen Prädikats, ob sich das Objekt
        /// schon in der Liste befindet. Ist dies nicht der Fall, so
        /// wird das Objekt hinzugefügt
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="list">Liste, zu der das Objekt hinzugefügt
        /// werden soll. </param>
        /// <param name="itemToBeAdded">Objekt, das hinzugefügt werden soll.</param>
        /// <param name="testFunction">Prädikat, mit dessen Hilfe getestet wird.</param>
        public static void AddIfNotExists<T>(
            IList<T> list, 
            T itemToBeAdded, 
            Func<T, T, bool> testFunction)
        {
            Ensure.IsNotNull(list);

            foreach (var element in list)
            {
                if (testFunction(itemToBeAdded, element))
                {
                    return;
                }
            }

            list.Add(itemToBeAdded);
        }

        /// <summary>
        /// Sortiert eine Liste mit Hilfe des übergebenen Comparers
        /// </summary>
        /// <typeparam name="T">Typ der in der Liste enthaltenen Elemente</typeparam>
        /// <param name="list">Zu sortierende Liste</param>
        /// <param name="comparer">Comparer to be used for sorting</param>
        /// <returns>Sortet list</returns>
        public static IList<T> Sort<T>(
            IEnumerable<T> list, 
            Comparison<T> comparer)
        {
            Ensure.IsNotNull(list);

            var copiedList = new List<T>(list);
            copiedList.Sort(comparer);
            return copiedList;
        }

        /// <summary>
        /// Entfernt alle Elemente aus der Liste <c>aiList</c>, die das 
        /// Prädikat <c>oPredicate</c> erfüllen.
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="list">Liste, die verändert werden soll</param>
        /// <param name="predicate">Predicate, which checks, if an 
        /// element has to be removed</param>
        /// <returns>Anzahl der entfernten Elemente</returns>
        public static int Remove<T>(IList<T> list, Predicate<T> predicate)
        {
            Ensure.IsNotNull(list);

            var copiedList = new List<T>(list);
            var position = 0;
            int removed = 0;
            foreach (var element in copiedList)
            {
                if (predicate(element))
                {
                    list.RemoveAt(position);
                    removed++;
                }
                else
                {
                    position++;
                }
            }

            return removed;
        }

        /// <summary>
        /// Returns the position of the first occurance of <c>needle</c>
        /// in the Array <c>hayStick</c>. The method <c>Equals</c> is used
        /// for testing. 
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="hayStick">Haystick with all elements</param>
        /// <param name="needle">Searched Needle</param>
        /// <param name="startPosition">Starting position of search</param>
        /// <returns>Position of first occurance</returns>
        public static int IndexOf<T>(T[] hayStick, T[] needle, int startPosition)
        {
            Ensure.IsNotNull(hayStick);
            Ensure.IsNotNull(needle);

            // Do standard error checking here.
            if (hayStick == null)
            {
                throw new ArgumentNullException("hayStick");
            }

            if (needle == null)
            {
                throw new ArgumentNullException("needle");
            }

            // Found?
            bool found = false;

            // Cycle through each byte of the searched.  Do not search past
            // searched.Length - find.Length bytes, since it's impossible
            // for the value to be found at that point.
            for (var index = startPosition; index <= hayStick.Length - needle.Length; ++index)
            {
                // Assume the values matched.
                found = true;

                // Search in the values to be found.
                for (var subIndex = 0L; subIndex < needle.Length; ++subIndex)
                {
                    // Check the value in the searched array vs the value
                    // in the find array.
                    if (!needle[subIndex].Equals(hayStick[index + subIndex]))
                    {
                        // The values did not match.
                        found = false;

                        // Break out of the loop.
                        break;
                    }
                }

                // If the values matched, return the index.
                if (found)
                {
                    // Return the index.
                    return index;
                }
            }

            // None of the values matched, return -1.
            return -1;
        }

        /// <summary>
        /// Stabile Sortierung mit Hilfe des Insertionsorts
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="list">Zu sortierende Liste</param>
        /// <param name="comparison">Comparison-Delegate used 
        /// for sorting</param>
        public static void InsertionSort<T>(IList<T> list, Comparison<T> comparison)
        {
            Ensure.IsNotNull(list);

            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (comparison == null)
            {
                throw new ArgumentNullException("comparison");
            }

            var count = list.Count;
            for (int j = 1; j < count; j++)
            {
                var key = list[j];

                var i = j - 1;
                for (; i >= 0 && comparison(list[i], key) > 0; i--)
                {
                    list[i + 1] = list[i];
                }

                list[i + 1] = key;
            }
        }

        /// <summary>
        /// Kopiert ein zweidimensionales Array
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="source">Zu kopierendes Array</param>
        /// <returns>Kopiertes Array</returns>
        public static T[,] Copy<T>(T[,] source)
        {
            Ensure.IsNotNull(source);

            int height = source.GetLength(0);
            int width = source.GetLength(1);

            var result = new T[height, width];
            for (int n = 0; n < height; n++)
            {
                for (int m = 0; m < width; m++)
                {
                    result[n, m] = source[n, m];
                }
            }

            return result;
        }

        /// <summary>
        /// Checks, if all elements in the enumeration fulfill the predicate. 
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="items">Items, which should be checked. </param>
        /// <param name="predicate">Predicate, which should be fulfilled</param>
        /// <returns>true, if all items fulfill the predicate</returns>
        [Obsolete("System.Linq.Enumerable.All()")]
        public static bool ForAll<T>(IEnumerable<T> items, Predicate<T> predicate)
        {
            Ensure.IsNotNull(items);

            foreach (var element in items)
            {
                if (!predicate(element))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Executes the <c>action</c> for each of the items in <c>items</c>
        /// parallel within threads. It is important that the actions are threadsafe.
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="items">Enumeration, whose elements should be
        /// executed.</param>
        /// <param name="action">Action, which should be executed</param>
        public static void ForeachParallel<T>(IEnumerable<T> items, Action<T> action)
        {
            Ensure.IsNotNull(items);

            ForeachParallel(
                items, 
                action, 
                Environment.ProcessorCount + 1);
        }

        /// <summary>
        /// Executes the <c>action</c> for each of the items in <c>items</c>
        /// parallel within threads. It is important that the actions are threadsafe.
        /// </summary>
        /// <typeparam name="T">Type of enumeration</typeparam>
        /// <param name="items">Enumeration, whose elements should be
        /// executed.</param>
        /// <param name="action">Action, which should be executed</param>
        /// <param name="numberOfThreads">Number of threads, that should
        /// be used</param>
        public static void ForeachParallel<T>(
            IEnumerable<T> items, 
            Action<T> action,
            int numberOfThreads)
        {
            Ensure.IsNotNull(items);

            // One thread, simple execution
            if (numberOfThreads == 1)
            {
                ListHelper.ForEach(items, action);
                return;
            }

            // Fills the stack with items
            var itemStack = new Stack<T>();
            foreach (var item in items)
            {
                itemStack.Push(item);
            }

            // Generates the threads
            var threads = new Thread[numberOfThreads];

            for (var n = 0; n < numberOfThreads; n++)
            {
                var threadStart = new ThreadStart(
                    () =>
                    {
                        while (true)
                        {
                            T item;
                            lock (itemStack)
                            {
                                if (itemStack.Count == 0)
                                {
                                    break;
                                }

                                item = itemStack.Pop();
                            }

                            action(item);
                        }
                    });
                var thread =                 
                    new Thread(threadStart);
                thread.Start();
                threads[n] = thread;
            }

            // Wait until all threads are joined
            for (var n = 0; n < numberOfThreads; n++)
            {
                threads[n].Join();
            }
        }

        /// <summary>
        /// Gets the index of an item matched by a predicate.
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="list">List of items</param>
        /// <param name="predicate">Predicate for finding the item</param>
        /// <returns>Index of item matching the predicate or -1 if not found. </returns>
        public static int IndexOf<T>(ICollection<T> list, Predicate<T> predicate)
        {
           Ensure.IsNotNull(list);

            var result = 0;

            foreach (var item in list)
            {
                if (predicate(item))
                {
                    return result;
                }

                result++;
            }

            return -1;
        }

        /// <summary>
        /// Finds the minimum value of an enumeration 
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="list">List to be queried</param>
        /// <param name="function">Function, which converts an 
        /// element to a comparable value</param>
        /// <returns>The minimum element</returns>
        public static T FindMin<T>(IEnumerable<T> list, Func<T, IComparable> function)
        {
            T smallest = default(T);
            IComparable smallestValue = null;
            bool first = true;

            foreach (var element in list)
            {
                var value = function(element);
                if (first
                    || smallestValue.CompareTo(value) > 0)
                {
                    smallestValue = value;
                    smallest = element;
                    first = false;
                }
            }

            return smallest;
        }

        /// <summary>
        /// Finds the minimum value of an enumeration 
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="list">List to be queried</param>
        /// <param name="function">Function, which converts an 
        /// element to a comparable value</param>
        /// <returns>The minimum element</returns>
        public static T FindMax<T>(IEnumerable<T> list, Func<T, IComparable> function)
        {
            Ensure.IsNotNull(list);

            T smallest = default(T);
            IComparable smallestValue = null;
            bool first = true;

            foreach (var element in list)
            {
                var value = function(element);
                if (first
                    || smallestValue.CompareTo(value) < 0)
                {
                    smallestValue = value;
                    smallest = element;
                    first = false;
                }
            }

            return smallest;
        }

        /// <summary>
        /// Creates a range of the given list. 
        /// </summary>
        /// <typeparam name="T">Type of elements in list</typeparam>
        /// <param name="list">List to be evaluated</param>
        /// <param name="start">Startposition of the range</param>
        /// <param name="end">Exclusive endposition of range</param>
        /// <returns>List, containing no, some or all elements of <c>list</c>.</returns>
        public static IList<T> Range<T>(IList<T> list, int start, int end)
        {
            Ensure.IsNotNull(list);

            Ensure.IsGreaterOrEqual(start, 0);
            Ensure.IsGreaterOrEqual(end, 0);

            var result = new List<T>();

            // Checks boundaries
            if (end <= start || start >= list.Count || end == 0)
            {
                return result;
            }

            // Truncate end
            end = Math.Min(end, list.Count);

            // Copy
            for (var n = start; n < end; n++)
            {
                result.Add(list[n]);
            }

            return result;
        }

        /// <summary>
        /// Returns an enumeration of integer starting by <c>start</c> and ending by
        /// <c>end</c>
        /// </summary>
        /// <param name="start">Inclusive start</param>
        /// <param name="end">Inclusive end</param>
        /// <returns>Enumeration of integers</returns>
        public static IEnumerable<int> Range(int start, int end)
        {
            if (end < start)
            {
                yield break;
            }

            for (var n = start; n <= end; n++)
            {
                yield return n;
            }
        }

        /// <summary>
        /// Returns an enumeration of integer starting by <c>start</c> and ending by
        /// <c>end</c>
        /// </summary>
        /// <param name="start">Inclusive start</param>
        /// <param name="end">Inclusive end</param>
        /// <returns>Enumeration of integers</returns>
        public static IEnumerable<long> Range(long start, long end)
        {
            if (end < start)
            {
                yield break;
            }

            for (var n = start; n <= end; n++)
            {
                yield return n;
            }
        }

        /// <summary>
        /// Checks, if both lists have the same content. Algorithm used:        /// 
        /// http://stackoverflow.com/questions/3669970/compare-two-listt-objects-for-equality-ignoring-order
        /// </summary>
        /// <typeparam name="T">Type of element</typeparam>
        /// <param name="list1">First list</param>
        /// <param name="list2">Second list</param>
        /// <returns>true, if both lists have the same elements</returns>
        public static bool HasSameElementsAs<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
#if DEBUG
            Ensure.That(list1.All(x => x != null), "list1 contains a null element");
            Ensure.That(list2.All(x => x != null), "list2 contains a null element");
#endif
            var cnt = new Dictionary<T, int>();
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }

            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="MathHelper.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using BurnSystems.Test;
    using System.Threading;

    /// <summary>
    /// Eine Klasse, die ein paar Hilfsfunktionen für die mathematischen 
    /// Routinen zur Verfügung stellt. 
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Eine Zufallsvariable
        /// </summary>
        private static ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));

        /// <summary>
        /// Gets a threadsafe random instance
        /// </summary>
        public static Random Random
        {
            get { return random.Value; }
        }

        /// <summary>
        /// Gets the earliest of both timespans
        /// </summary>
        /// <param name="timeSpan1">The first timespan</param>
        /// <param name="timeSpan2">The second timespan</param>
        /// <returns>The earliest timespan</returns>
        public static TimeSpan Min(TimeSpan timeSpan1, TimeSpan timeSpan2)
        {
            if (timeSpan1.TotalMilliseconds > timeSpan2.TotalMilliseconds)
            {
                return timeSpan2;
            }
            else
            {
                return timeSpan1;
            }
        }

        /// <summary>
        /// Gets the latest of both timespans
        /// </summary>
        /// <param name="timeSpan1">The first timespan</param>
        /// <param name="timeSpan2">The second timespan</param>
        /// <returns>The latest timespan</returns>
        public static TimeSpan Max(TimeSpan timeSpan1, TimeSpan timeSpan2)
        {
            if (timeSpan1.TotalMilliseconds < timeSpan2.TotalMilliseconds)
            {
                return timeSpan2;
            }
            else
            {
                return timeSpan1;
            }
        }

        /// <summary>
        /// Gets the earliest of both timestamps
        /// </summary>
        /// <param name="dateTime1">The first timestamp</param>
        /// <param name="dateTime2">The second timestamp</param>
        /// <returns>The earlier timestamp</returns>
        public static DateTime Min(DateTime dateTime1, DateTime dateTime2)
        {
            if (dateTime1.Ticks > dateTime2.Ticks)
            {
                return dateTime2;
            }
            else
            {
                return dateTime1;
            }
        }

        /// <summary>
        /// Gets the latest of both timestamps
        /// </summary>
        /// <param name="dateTime1">The first timestamp</param>
        /// <param name="dateTime2">The second timestamp</param>
        /// <returns>The later timespamp</returns>
        public static DateTime Max(DateTime dateTime1, DateTime dateTime2)
        {
            if (dateTime1.Ticks < dateTime2.Ticks)
            {
                return dateTime2;
            }
            else
            {
                return dateTime1;
            }
        }

        /// <summary>
        /// Gets the minimum value of all parameters
        /// </summary>
        /// <param name="values">Array of parameters</param>
        /// <returns>Minimum value of all parameter</returns>
        public static double Min(params double[] values)
        {
            return values.Min();
        }

        /// <summary>
        /// Gets the minimum value of all parameters
        /// </summary>
        /// <param name="values">Array of parameters</param>
        /// <returns>Minimum value of all parameter</returns>
        public static double Max(params double[] values)
        {
            return values.Max();
        }

        /// <summary>
        /// Gets the smallest object in enumeration.
        /// </summary>
        /// <typeparam name="T">Type of objects to be enumerated</typeparam>
        /// <param name="objects">Objects to be compared</param>
        /// <returns>The smallest object in enumeration or <c>default(T)</c>, 
        /// if there are no values in enumeration</returns>
        public static T Min<T>(IEnumerable<T> objects) where T : IComparable<T>
        {
            bool first = true;
            T result = default(T);

            foreach (var obj in objects)
            {
                if (first)
                {
                    result = obj;
                    first = false;
                }
                else
                {
                    result = (result.CompareTo(obj) == -1) ? result : obj;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the biggest object in enumeration.
        /// </summary>
        /// <typeparam name="T">Type of objects to be enumerated</typeparam>
        /// <param name="objects">Objects to be compared</param>
        /// <returns>The biggest object in enumeration or <c>default(T)</c>, 
        /// if there are no values in enumeration</returns>
        public static T Max<T>(IEnumerable<T> objects) where T : IComparable<T>
        {
            var first = true;
            T result = default(T);

            foreach (var obj in objects)
            {
                if (first)
                {
                    result = obj;
                    first = false;
                }
                else
                {
                    result = (result.CompareTo(obj) == 1) ? result : obj;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns random number with gaussian propabilty
        /// </summary>
        /// <param name="average">Expected average</param>
        /// <param name="variance">Expected variance</param>
        /// <returns>Determined random number</returns>
        public static double GetNextGaussian(double average, double variance)
        {
            while (true)
            {
                Ensure.IsGreaterOrEqual(variance, 0.0);
                var d1 = Random.NextDouble();
                var d2 = Random.NextDouble();

                // Algorithmus aus http://de.wikipedia.org/wiki/Normalverteilung
                var dV = (((2 * d1) - 1) * ((2 * d1) - 1))
                    + (((2 * d2) - 1) * ((2 * d2) - 1));
                if (dV >= 1)
                {
                    continue;
                }

                var randomNumber = ((2 * d1) - 1) * Math.Sqrt(-2 * Math.Log(dV) / dV);
                randomNumber *= Math.Sqrt(variance);
                return randomNumber + average;                
            }
        }
        
        /// <summary>
        /// Formatiert einen Timespan so, dass er dem Format
        /// HH:MM:SS entspricht
        /// </summary>
        /// <param name="timeSpan">Zu formatierender Timespan</param>
        /// <returns>String mit dem oben genannten Format</returns>
        public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan > TimeSpan.MaxValue - TimeSpan.FromSeconds(2) ||
                timeSpan < TimeSpan.MinValue + TimeSpan.FromSeconds(2))
            {
                return LocalizationBS.TimeSpan_MaxValue;
            }

            int totalSeconds = (int)Math.Round(timeSpan.TotalSeconds);
            int seconds = totalSeconds % 60;
            int minutes = (totalSeconds / 60) % 60;
            int hours = totalSeconds / 3600;

            return String.Format(
                CultureInfo.InvariantCulture,
                "{0}:{1}:{2}",
                MakeTwoDigits(hours),
                MakeTwoDigits(minutes),
                MakeTwoDigits(seconds));
        }

        /// <summary>
        /// Gibt eine zwei- oder mehrstellige Ziffer.
        /// </summary>
        /// <param name="digit">Ziffer, die umgewandelt werden soll.</param>
        /// <returns>Zahl im Format XX.</returns>
        private static string MakeTwoDigits(int digit)
        {
            if (digit < 10)
            {
                return string.Format(CultureInfo.InvariantCulture, "0{0}", digit);
            }

            return digit.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Threadsafe class of random. 
        /// </summary>
        private class RandomThreadSafe : Random
        {
            /// <summary>
            /// Object for synchronisation
            /// </summary>
            private object syncObject = new object();

            [ThreadStatic]
            private static Random localRandomGenerator;

            private Random LocalRandomGenerator
            {
                get
                {
                    var inst = localRandomGenerator;
                    if (inst == null)
                    {
                        int seed;
                        lock (syncObject)
                        {
                            seed = base.Next();
                        }
                        localRandomGenerator = inst = new Random(seed);
                    }

                    return inst;
                }
            }

            /// <summary>
            /// Calls Random.NextDouble()
            /// </summary>
            /// <returns>Result of Call</returns>
            public override double NextDouble()
            {
                return this.LocalRandomGenerator.NextDouble();
            }

            /// <summary>
            /// Calls Random.Next()
            /// </summary>
            /// <returns>Result of Call</returns>
            public override int Next()
            {
                return this.LocalRandomGenerator.Next();
            }

            /// <summary>
            /// Calls Random.Next()
            /// </summary>
            /// <param name="maxValue">Parameter for Random.Next</param>
            /// <returns>Result of Call</returns>
            public override int Next(int maxValue)
            {
                return this.LocalRandomGenerator.Next(maxValue);
            }

            /// <summary>
            /// Calls Random.Next()            
            /// </summary>
            /// <param name="minValue">First Parameter for Random.Next</param>
            /// <param name="maxValue">Second Parameter for Random.Next</param>
            /// <returns>Result of Call</returns>
            public override int Next(int minValue, int maxValue)
            {
                return this.LocalRandomGenerator.Next(minValue, maxValue);
            }

            /// <summary>
            /// Calls Random.NextBytes()
            /// </summary>
            /// <param name="buffer">Parameter for buffer</param>
            public override void NextBytes(byte[] buffer)
            {
                this.LocalRandomGenerator.NextBytes(buffer);
            }
        }
    }
}

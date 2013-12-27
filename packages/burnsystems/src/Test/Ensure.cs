//-----------------------------------------------------------------------
// <copyright file="Ensure.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Test
{
    using System;
    using System.Globalization;
    using BurnSystems.Collections;
    using System.Linq.Expressions;

    /// <summary>
    /// Mit Hilfe dieser Hilfsklasse kann überprüft werden, ob 
    /// ein bestimmter Zustand eingehalten wird. Im Prinzip entspricht 
    /// diese Klasse der Debug.Assert-Funktionalität, ist nur um einiges
    /// flexibler. 
    /// <para>
    /// Scheitert eine Abfrage, so wird eine EnsureFailedException geworfen. 
    /// </para>
    /// </summary>
    public static class Ensure
    {
        /// <summary>
        /// Wirft die 'EnsureFailedException'-Ausnahme
        /// </summary>
        public static void Fail()
        {
            throw new EnsureFailedException("Fail");
        }

        /// <summary>
        /// Wirft die 'EnsureFailedException'-Ausnahme
        /// </summary>
        /// <param name="failText">The failtext, which is thrown
        /// with the exception</param>
        public static void Fail(string failText)
        {
            throw new EnsureFailedException("Fail: " + failText);
        }

        /// <summary>
        /// Versichert, dass das angegebene Objekt null ist. 
        /// </summary>
        /// <param name="value">Zu prüfendes Objekt</param>
        public static void IsNull(object value)
        {
            if (value != null)
            {
                throw new EnsureFailedException("IsNull");
            }
        }

        /// <summary>
        /// Versichert, dass das angegebene Objekt nicht null ist
        /// </summary>
        /// <param name="value">Zu prüfendes Objekt</param>
        public static void IsNotNull(object value)
        {
            if (value == null)
            {
                throw new EnsureFailedException("IsNotNull");
            }
        }

        /// <summary>
        /// Versichert, dass das angegebene Objekt nicht null ist
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <param name="errorText">Errortext, which is thrown, if
        /// check fails</param>
        public static void IsNotNull(object value, string errorText)
        {
            if (value == null)
            {
                throw new EnsureFailedException("IsNotNull: " + errorText);
            }
        }

        /// <summary>
        /// Prüft, ob der angegebene Wert true ist
        /// </summary>
        /// <param name="value">Zu überprüfende Wert</param>
        public static void IsTrue(bool value)
        {
            if (!value)
            {
                throw new EnsureFailedException("IsTrue");
            }
        }

        /// <summary>
        /// Checks value is true. If value is not true, the action will be executed
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <param name="action">Action to be executed.</param>
        public static void IsTrue(bool value, Action action)
        {
            if (!value)
            {
                action();
            }
        }

        /// <summary>
        /// Prüft, ob der angegebene Wert true ist
        /// </summary>
        /// <param name="value">Zu überprüfende Wert</param>
        public static void That(bool value)
        {
            if (!value)
            {
                throw new EnsureFailedException("That");
            }
        }

        /// <summary>
        /// Prüft, ob der angegebene Wert true ist
        /// </summary>
        /// <param name="expression">Expression to be checked</param>
        public static void That(Expression<Func<bool>> expression)
        {
            if (!expression.Compile()())
            {
                throw new EnsureFailedException("That: " + expression.ToString());
            }
        }

        /// <summary>
        /// Prüft, ob der angegebene Wert true ist
        /// </summary>
        /// <param name="value">Zu überprüfende Wert</param>
        /// <param name="message">Message to be shown</param>
        public static void That(bool value, string message)
        {
            if (!value)
            {
                throw new EnsureFailedException(message);
            }
        }

        /// <summary>
        /// Checks value is true. If value is not true, the action will be executed
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <param name="action">Action to be executed.</param>
        public static void That(bool value, Action action)
        {
            if (!value)
            {
                action();
            }
        }
        
        /// <summary>
        /// Prüft, ob der angegebene Wert true ist
        /// </summary>
        /// <param name="value">Zu überprüfende Wert</param>
        /// <param name="errorText">Error text, if check fails</param>
        public static void IsTrue(bool value, string errorText)
        {
            if (!value)
            {
                throw new EnsureFailedException("IsTrue: " + errorText);
            }
        }

        /// <summary>
        /// Prüft, ob der angegebene Wert false ist
        /// </summary>
        /// <param name="value">Zu überprüfende Wert</param>
        public static void IsFalse(bool value)
        {
            if (value)
            {
                throw new EnsureFailedException("IsFalse");
            }
        }

        /// <summary>
        /// Prüft, ob der angegebene Wert false ist
        /// </summary>
        /// <param name="value">Zu überprüfende Wert</param>
        /// <param name="errorText">Error text, if check fails</param>
        public static void IsFalse(bool value, string errorText)
        {
            if (value)
            {
                throw new EnsureFailedException("IsFalse: " + errorText);
            }
        }

        #region AreEqual
        
        /// <summary>
        /// Überprüft, ob die beiden Objekte gleich sind. Zum Vergleich
        /// wird der ==-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void AreEqual<T>(T value, T reference) 
        {
            if (value.Equals(reference))
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "Is: {0}, Should: {1}", 
                        value.ToString(), 
                        reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob die beiden Objekte gleich sind. Zum Vergleich
        /// wird der ==-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        /// <param name="text">Error text</param>
        public static void AreEqual<T>(T value, T reference, string text)
        {
            if (value.Equals(reference))
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "Is: {0}, Should: {1}", 
                        value.ToString(), 
                        reference.ToString(), 
                        text));
            }
        }

        #endregion

        #region AreNotEqual

        /// <summary>
        /// Überprüft, ob die beiden Objekte ungleich sind. Zum Vergleich
        /// wird der !=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void AreNotEqual<T>(T value, T reference)
        {
            if (!value.Equals(reference))
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} != {1}", 
                        value.ToString(), 
                        reference.ToString()));
            }
        }

        #endregion

        #region IsGreater

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreater(short value, short reference)
        {
            if (value > reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} > {1}", 
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreater(int value, int reference)
        {
            if (value > reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} > {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreater(long value, long reference)
        {
            if (value > reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} > {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreater(float value, float reference)
        {
            if (value > reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} > {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreater(double value, double reference)
        {
            if (value > reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} > {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreater(decimal value, decimal reference)
        {
            if (value > reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} > {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        #endregion

        #region IsGreaterOrEqual

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(short value, short reference)
        {
            if (value >= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} >= {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(int value, int reference)
        {
            if (value >= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} >= {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(long value, long reference)
        {
            if (value >= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} >= {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(float value, float reference)
        {
            if (value >= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} >= {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(double value, double reference)
        {
            if (value >= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} >= {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(decimal value, decimal reference)
        {
            if (value >= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} >= {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        #endregion

        #region IsSmaller

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmaller(short value, short reference)
        {
            if (value < reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} < {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmaller(int value, int reference)
        {
            if (value < reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} < {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmaller(long value, long reference)
        {
            if (value < reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} < {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmaller(float value, float reference)
        {
            if (value < reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} < {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmaller(double value, double reference)
        {
            if (value < reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} < {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmaller(decimal value, decimal reference)
        {
            if (value < reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} < {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        #endregion

        #region IsSmallerOrEqual

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(short value, short reference)
        {
            if (value <= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} <= {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(int value, int reference)
        {
            if (value <= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} <= {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(long value, long reference)
        {
            if (value <= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} <= {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(float value, float reference)
        {
            if (value <= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} <= {1}", 
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(double value, double reference)
        {
            if (value <= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} <= {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(decimal value, decimal reference)
        {
            if (value <= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "{0} <= {1}",
                        value.ToString(CultureInfo.CurrentCulture),
                        reference.ToString(CultureInfo.CurrentCulture)));  
            }
        }

        #endregion

        /// <summary>
        /// Versichert, dass eine Ausnahme geworfen wird
        /// </summary>
        /// <typeparam name="T">Typ der zu werfenden Ausnahme</typeparam>
        /// <param name="function">Function, which is called. The 
        /// Exception is expected to be thrown from the function</param>
        public static void ThrowsException<T>(Action function) where T : Exception
        {
            try
            {
                function();
            }
            catch (T)
            {
                // Alles OK
                return;
            }

            throw new EnsureFailedException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "ThrowsException<{0}>",
                    typeof(T).FullName));
        }

        /// <summary>
        /// Checks, if the given object is from a specific type
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <param name="type">Type to be checked</param>
        public static void IsType(object value, Type type)
        {
            if (!type.IsInstanceOfType(value))
            {
                if (value == null)
                {
                    throw new EnsureFailedException("null not type of " + type.FullName);
                }
                else
                {
                    throw new EnsureFailedException(value.ToString() + " not type of " + type.FullName);
                }
            }
        }

        /// <summary>
        /// Checks if a value is between the given values. 
        /// The border are inclusive. 
        /// This means <c>min &lt;= value &lt;= max</c> is OK
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        public static void IsBetween(int value, int min, int max)
        {
            if (!(min <= value && value <= max))
            {
                throw new EnsureFailedException();
            }
        }
    }
}

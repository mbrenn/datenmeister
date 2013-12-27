//-----------------------------------------------------------------------
// <copyright file="DcssConverter.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Parser.Dcss
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using BurnSystems.Test;

    /// <summary>
    /// Kontext, in dem sich der Parser aktuell befindet. 
    /// Die Namen richten sich nach der Spezifikation gemäß
    /// CSS 2.1 Kapitel 4.1.1
    /// </summary>
    internal enum ParsingContext
    {
        /// <summary>
        /// Stylesheet parsing mode
        /// </summary>
        Stylesheet,

        /// <summary>
        /// Statement parsing mode
        /// </summary>
        Statement,

        /// <summary>
        /// At rule parsing mode
        /// </summary>
        AtRule,

        /// <summary>
        /// Block parsing mode
        /// </summary>
        Block,

        /// <summary>
        /// Ruleset parsing mode
        /// </summary>
        Ruleset,

        /// <summary>
        /// Selector parsing mode
        /// </summary>
        Selector,

        /// <summary>
        /// Declaration parsing mode
        /// </summary>
        Declaration,

        /// <summary>
        /// Property parsing mode
        /// </summary>
        Property,

        /// <summary>
        /// Value parsing mode
        /// </summary>
        Value,

        /// <summary>
        /// Any parsing mode
        /// </summary>
        Any
    }

    /// <summary>
    /// Dieser Konvertierer verarbeitet eine Dcss-Datei und gibt
    /// eine CSS 2.1 konforme CSS-Datei zurück.
    /// </summary>
    public class DcssConverter
    {
        /// <summary>
        /// Flag, ob eine Debug-Ausgabe auf die Konsole
        /// gebracht werden soll. 
        /// </summary>
        private bool debug;

        /// <summary>
        /// Variables defined in dcss
        /// </summary>
        private Dictionary<string, string> variables =
            new Dictionary<string, string>();

        /// <summary>
        /// Speichert das Resultat des Konverters
        /// </summary>
        private StringBuilder result = new StringBuilder();
        
        /// <summary>
        /// Dieser Textreader enthält den Quellstring
        /// </summary>
        private string inputString;

        /// <summary>
        /// Aktuelle Position während des Parsevorgangs
        /// </summary>
        private int currentPosition;

        /// <summary>
        /// Dieses Objekt wird geworfen, wenn ein neues Ruleset-Objekt
        /// geparst wurde. 
        /// </summary>
        private RulesetPropertyValueHandler rulesetPropertyValueParsed;

        /// <summary>
        /// Delegate, which is called, when a property was parsed
        /// </summary>
        /// <param name="property">Property for ruleset</param>
        /// <param name="value">Value for ruleset</param>
        private delegate void RulesetPropertyValueHandler(
            string property,
            string value);

        // <summary>
        // Dies ist der zu parsende Reststring. Diese Funktion
        // ist Hauptsächlich für Debugzwecke eingerichtet
        // </summary>
        /*String RestString
        {
            get
            {
                if (_Input != null &&
                    _CurrentPosition >= 0 && _CurrentPosition < _Input.Length)
                {
                    return _Input.Substring(_CurrentPosition);
                }
                return String.Empty;
            }
        }*/

        /// <summary>
        /// Gets or sets a value indicating whether debugstrings
        /// should be sent to console
        /// </summary>
        public bool DebugActive
        {
            get { return this.debug; }
            set { this.debug = value; }
        }

        /// <summary>
        /// Konvertiert eine Datei
        /// </summary>
        /// <param name="fileContent">Die zu lesende Datei</param>
        /// <returns>Gibt die resultierende Datei als String 
        /// zurück</returns>
        public string Convert(string fileContent)
        {
            if (fileContent == null)
            {
                throw new ArgumentNullException("strFile");
            }

            this.inputString = CommentParser.StripStarComments(fileContent);

            this.ConvertStylesheet();

            return this.result.ToString();
        }

        /// <summary>
        /// Konvertiert den Inhalt eines Textreaders und gibt 
        /// einen neuen Textreader mit dem Inhalt der CSS-Datei zurück
        /// </summary>
        /// <param name="textReader">Textreader, der die DCSS-Datei
        /// gespeichert hat</param>
        /// <returns>Textreader, der die resultierende CSS-Datei 
        /// speichert.</returns>
        public TextReader Convert(TextReader textReader)
        {            
            return new StringReader(this.Convert(textReader.ReadToEnd()));
        }

        /// <summary>
        /// Diese Funktion gibt true zurück, wenn der übergebene Character
        /// einen 'any'-Wert gemäß CSS 2.1-Spezifikation entspricht. 
        /// </summary>
        /// <param name="character">Zu prüfendes Zeichen</param>
        /// <remarks>
        /// Gemäß CSS-Spezifikation: 
        /// <code>
        /// any         
        /// : [ IDENT | NUMBER | PERCENTAGE | DIMENSION | STRING
        /// | DELIM | URI | HASH | UNICODE-RANGE | INCLUDES
        /// | DASHMATCH | FUNCTION S* any* ')' 
        /// | '(' S* any* ')' | '[' S* any* ']' ] S*;
        /// </code>
        /// </remarks>
        /// <returns>Returns, if character is one of the set 
        /// of 'any'-Characters</returns>
        private static bool IsAny(char character)
        {
            // ANY ist alles, außer:
            // ATKEYWORD, \;, \{, \}, 
            // Einfach zu prüfen.
            return !(character == '@' || character == '{' ||
                character == '}' || character == ';');
        }

        /// <summary>
        /// Parst den String als Stylesheet und ruft die jeweiligen
        /// Kontext-Funktionen auf. 
        /// </summary>
        private void ConvertStylesheet()
        {
            var length = this.inputString.Length;
            this.result.Append(this.GetWhitespaces());
            while (this.currentPosition < length)
            {
                var currentCharacter = this.inputString[this.currentPosition];

                if (currentCharacter == '@')
                {
                    this.result.Append(this.ParseAtRule());
                }
                else if (IsAny(currentCharacter))
                {
                    this.result.Append(this.ParseRuleset());
                }
                else
                {
                    this.result.Append(currentCharacter);
                    this.currentPosition++;
                }

                this.result.Append(this.GetWhitespaces());
            }
        }

        /// <summary>
        /// Liest eine AtRule ein
        /// </summary>
        /// <returns>Returns the at rule</returns>
        private string ParseAtRule()
        {
            this.currentPosition++;

            // Habe nun das Ende der At Rule
            var atruleName = this.GetIdent();

            // Holt sich nun den Rest
            string whitespaces = this.GetWhitespaces();

            // Holt sich das 'Any'
            string any = this.GetAny();

            // Überspringt weitere Whitespaces
            string whitespaces2 = this.GetWhitespaces();

            // Überprüft, ob das aktuelle Symbol ein ';' oder ein 
            // Block ist
            string rest;
            if (this.inputString[this.currentPosition] == ';')
            {
                // Semikolon
                rest = ";";
            }
            else
            {
                if (atruleName.Trim() == "dcssdefine")
                {
                    this.ParseDCSSDefineBlock();
                    return String.Empty;
                }
                else
                {
                    // Block
                    rest = this.GetBlock();
                }
            }

            this.WriteDebug("At-Rule: " + atruleName);

            // Gibt die gesamte AtRule zurück
            return string.Format(
                CultureInfo.InvariantCulture,
                "@{0}{1}{2}{3}{4}",
                atruleName,
                whitespaces,
                any,
                whitespaces2,
                rest);
        }

        /// <summary>
        /// Diese Funktion gibt einen kompletten Block zurück, 
        /// es wird sich nur auf die geschweiften Klammern, eventuellen
        /// Kommentaren und Anführungszeichen. Der Inhalt selbst wird
        /// nicht verstanden. 
        /// </summary>
        /// <returns>Returns the block</returns>
        private string GetBlock()
        {
            var block = new StringBuilder();

            // Aktuelle Blocktiefe
            int blockDepth = 1;

            // Flag, ob sich der Parser gerade im Quote befindet
            bool quoteActive = false;

            // Flag, ob das letzte Zeichen ein Escape-Character war
            bool escaped = false;
            
            // Das erste Zeichen muss ein '{' sein. 
            if (this.inputString[this.currentPosition] != '{')
            {
                Debug.Fail("inputString[currentPosition ] != '{'");
            }

            this.currentPosition++;
            block.Append('{');

            while (this.currentPosition < this.inputString.Length)
            {
                var currentCharacter = this.inputString[this.currentPosition];

                if (quoteActive)
                {
                    // Ist in quote. 
                    if (escaped)
                    {
                        escaped = false;
                    }
                    else if (currentCharacter == '"')
                    {
                        quoteActive = false;
                    }
                    else if (currentCharacter == '\\')
                    {
                        escaped = true;
                    }
                }
                else if (currentCharacter == '}')
                {
                    blockDepth--;
                    if (blockDepth <= 0)
                    {
                        break;
                    }
                }
                else if (currentCharacter == '{')
                {
                    blockDepth++;
                }

                block.Append(currentCharacter);
                this.currentPosition++;
            }

            var blockString = block.ToString();
            this.WriteDebug("Block: " + blockString);

            return blockString;
        }

        /// <summary>
        /// Parst ein Regelwerk
        /// </summary>
        /// <returns>Parsed ruleset</returns>
        private string ParseRuleset()
        {
            // Lädt den Selektor ein
            var selector = this.GetSelector();

            this.WriteDebug("Selector: " + selector);

            var rules = this.ParseRulesetBlock();

            return String.Format(
                CultureInfo.InvariantCulture,
                "{0}{1}{2}", 
                selector, 
                '{', 
                rules.ToString());
        }

        /// <summary>
        /// Parst den Block des Regelwerks
        /// </summary>
        /// <returns>Der zu parsende Rulesetblock</returns>
        private StringBuilder ParseRulesetBlock()
        {
            // Nun folgt die OpenBracket
            Ensure.AreEqual(this.inputString[this.currentPosition], '{');

            this.currentPosition++;

            // Nun folgen die einzelnen Eigenschaften. 
            // Diese sind relativ simpel zu parsen: 
            // S* property S* ':' S* value;
            var rules = new StringBuilder();

            while (this.currentPosition < this.inputString.Length)
            {
                rules.Append(this.GetWhitespaces());
                if (this.currentPosition >= this.inputString.Length)
                {
                    // Ende
                    break;
                }

                if (this.inputString[this.currentPosition] == '}')
                {
                    // Schließende Klammer.
                    rules.Append('}');
                    this.currentPosition++;
                    break;
                }
                else if (this.inputString[this.currentPosition] == ';')
                {
                    rules.Append(';');
                    this.currentPosition++;
                }
                else
                {
                    // Suche nun den Doppelpunkt
                    var colon = this.inputString.IndexOf(':', this.currentPosition);
                    Debug.Assert(colon != -1, "No Colon in property found");
                    if (colon == -1)
                    {
                        // Abbruch, um Endlosschleife zu verhindern
                        this.currentPosition = this.inputString.Length;
                        break;
                    }

                    // Suche nun Das Semikolon oder die schließende Klammer
                    int semikolon = this.inputString.IndexOf(';', colon);
                    int closingBracket = this.inputString.IndexOf('}', colon);

                    int endValue;
                    if (semikolon == -1)
                    {
                        endValue = closingBracket;
                    }
                    else
                    {
                        endValue = Math.Min(closingBracket, semikolon);
                    }
                    
                    Ensure.IsTrue(endValue != -1, "No end in Converter");
                    if (endValue == -1 || colon == -1)
                    {
                        // Abbruch, um Endlosschleife zu verhindern
                        this.currentPosition = this.inputString.Length;
                        break;
                    }

                    // Nun werden die Daten geholt. 
                    string property =
                        this.inputString.Substring(
                            this.currentPosition,
                            colon - this.currentPosition);
                    string value =
                        this.inputString.Substring(
                            colon + 1,
                            endValue - colon - 1);

                    if (this.rulesetPropertyValueParsed != null)
                    {
                        this.rulesetPropertyValueParsed(property, value);
                    }

                    this.WriteDebug(property + ": " + value);

                    // Füge nun die Variablen ein. 
                    if (value.IndexOf('[') != -1)
                    {
                        foreach (var pair in this.variables)
                        {
                            value =
                                value.Replace(
                                    String.Format(CultureInfo.InvariantCulture, "[{0}]", pair.Key),
                                    pair.Value);
                        }
                    }

                    // Und baue den Spaß wieder zusammen
                    rules.AppendFormat("{0}:{1}", property, value);

                    this.currentPosition = endValue;
                }
            }

            return rules;
        }

        /// <summary>
        /// Parst einen Block und setzt die Variablen
        /// </summary>
        private void ParseDCSSDefineBlock()
        {
            this.rulesetPropertyValueParsed =
                delegate(string property, string value)
                {
                    this.WriteDebug("Set Variable: " + property);
                    this.variables[property.Trim()] = value.Trim();
                };

            this.ParseRulesetBlock();
        }

        /// <summary>
        /// Gibt den Selektor zurück
        /// </summary>
        /// <returns>Text of selector</returns>
        private string GetSelector()
        {
            int openBracket =
                this.inputString.IndexOf('{', this.currentPosition);

            if (openBracket == -1)
            {
                Debug.Fail("No open bracket after Selector");
                return string.Empty;
            }

            var selector =
                this.inputString.Substring(
                this.currentPosition,
                     openBracket - this.currentPosition);
            this.currentPosition = openBracket;
            return selector;
        }

        /// <summary>
        /// Gibt 'ANY' zurück
        /// </summary>
        /// <returns>Text of any</returns>
        private string GetAny()
        {
            return this.GetByPredicate(
                x => IsAny(x));
        }

        /// <summary>
        /// Gibt einen Identifikationstring zurück
        /// </summary>
        /// <returns>Returns the identifier</returns>
        private string GetIdent()
        {
            return
                this.GetByPredicate(
                    x => Char.IsLetterOrDigit(x) || (int)x > 177);
        }

        /// <summary>
        /// Gets the whitespaces
        /// </summary>
        /// <returns>String with whitespaces</returns>
        private string GetWhitespaces()
        {
            return
               this.GetByPredicate(
                   x =>
                       x == ' '
                       || x == '\r'
                       || x == '\t'
                       || x == '\n'
                       || x == '\f');
        }

        /// <summary>
        /// Gets the next values, until predicate for character is false
        /// </summary>
        /// <param name="predicate">Used predicate</param>
        /// <returns>String till predicate</returns>
        private string GetByPredicate(Predicate<char> predicate)
        {
            var identification = new StringBuilder();
            while (this.currentPosition < this.inputString.Length)
            {
                var character = this.inputString[this.currentPosition];
                if (predicate(character))
                {
                    identification.Append(character);
                }
                else
                {
                    break;
                }

                this.currentPosition++;
            }

            return identification.ToString();
        }

        /// <summary>
        /// Gibt bei Bedarf einen Debugstring auf die Konsole heraus
        /// </summary>
        /// <param name="message">Message for debug</param>
        private void WriteDebug(string message)
        {
            if (this.debug)
            {
                Console.WriteLine(message);
            }
        }
    }
}
